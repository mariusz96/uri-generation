using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using System.Reflection;
using UriGeneration.AspNetWebStack;
using UriGeneration.Internal.Abstractions;

namespace UriGeneration.Internal
{
    internal class ValuesExtractor : IValuesExtractor
    {
        private static readonly Func<BindingSource?, bool> BindingSourceFilterDelegate =
            BindingSourceFilter;
        private static readonly ParameterExpression FakeParameter =
            Expression.Parameter(typeof(object), null);
        private static readonly MemoryCacheEntryOptions MethodCacheEntryOptions =
            new() { Size = 1 };

        private readonly IMethodCacheAccessor _methodCacheAccessor;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorsProvider;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly UriGenerationOptions _globalOptions;
        private readonly ILogger<ValuesExtractor> _logger;

        public ValuesExtractor(
            IMethodCacheAccessor methodCacheAccessor,
            IActionDescriptorCollectionProvider actionDescriptorsProvider,
            IModelMetadataProvider modelMetadataProvider,
            IOptions<UriGenerationOptions> globalOptionsAccessor,
            ILogger<ValuesExtractor> logger)
        {
            if (methodCacheAccessor == null)
            {
                throw new ArgumentNullException(nameof(methodCacheAccessor));
            }

            if (actionDescriptorsProvider == null)
            {
                throw new ArgumentNullException(nameof(actionDescriptorsProvider));
            }

            if (modelMetadataProvider == null)
            {
                throw new ArgumentNullException(nameof(modelMetadataProvider));
            }

            if (globalOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(globalOptionsAccessor));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _methodCacheAccessor = methodCacheAccessor;
            _actionDescriptorsProvider = actionDescriptorsProvider;
            _modelMetadataProvider = modelMetadataProvider;
            _globalOptions = globalOptionsAccessor.Value;
            _logger = logger;
        }

        public bool TryExtractValues<TController>(
            HttpContext? httpContext,
            Expression<Action<TController>> expression,
            UriOptions? options,
            [NotNullWhen(true)] out Values? values)
                where TController : class
        {
            try
            {
                values = default;

                if (!TryExtractMethodCall(expression.Body, out var methodCall))
                {
                    return false;
                }

                var method = ExtractMethod(methodCall);
                var controller = ExtractController<TController>();

                var actionDescriptors = _actionDescriptorsProvider.ActionDescriptors;

                var methodCache = _methodCacheAccessor.Cache;
                var key = new MethodCacheKey(
                    method,
                    controller,
                    actionDescriptors.Version);

                bool? bypassMethodCache = options?.BypassMethodCache
                    ?? _globalOptions.BypassMethodCache;

                if (bypassMethodCache is not true
                    && methodCache.TryGetValue(key, out MethodCacheEntry? entry))
                {
                    if (entry!.IsValid)
                    {
                        _logger.ValidCacheEntryRetrieved(entry.ActionDescriptor);

                        if (!ValidateBoundProperties(entry.ActionDescriptor))
                        {
                            return false;
                        }

                        var entryRouteValues = ExtractRouteValues(
                            httpContext,
                            entry.ActionDescriptor,
                            methodCall.Arguments,
                            options);

                        values = new Values(
                            entry.ActionDescriptor.ActionName,
                            entry.ActionDescriptor.ControllerName,
                            entryRouteValues);

                        _logger.ValuesExtracted();
                        return true;
                    }
                    else
                    {
                        _logger.InvalidCacheEntryRetrieved();
                        return false;
                    }
                }

                if (!ValidateMethodConcreteness(method, controller)
                    || !TryExtractActionDescriptor(method, actionDescriptors, out var descriptor))
                {
                    if (bypassMethodCache is not true)
                    {
                        var invalidEntry = MethodCacheEntry.Invalid();
                        methodCache.Set(key, invalidEntry, MethodCacheEntryOptions);
                    }

                    return false;
                }

                if (!ValidateBoundProperties(descriptor))
                {
                    return false;
                }

                var routeValues = ExtractRouteValues(
                    httpContext,
                    descriptor,
                    methodCall.Arguments,
                    options);

                if (bypassMethodCache is not true)
                {
                    var validEntry = MethodCacheEntry.Valid(descriptor);
                    methodCache.Set(key, validEntry, MethodCacheEntryOptions);
                }

                values = new Values(
                    descriptor.ActionName,
                    descriptor.ControllerName,
                    routeValues);

                _logger.ValuesExtracted();
                return true;
            }
            catch (Exception exception)
            {
                values = default;

                _logger.ValuesNotExtracted(expression, exception);
                return false;
            }
        }

        private bool TryExtractMethodCall(
            Expression expressionBody,
            [NotNullWhen(true)] out MethodCallExpression? methodCall)
        {
            methodCall = expressionBody as MethodCallExpression;

            if (methodCall == null)
            {
                _logger.MethodCallNotExtracted(expressionBody);
                return false;
            }

            _logger.MethodCallExtracted();
            return true;
        }

        private MethodInfo ExtractMethod(MethodCallExpression methodCall)
        {
            var method = methodCall.Method;

            _logger.MethodExtracted();
            return method;
        }

        private Type ExtractController<TController>()
            where TController : class
        {
            var controller = typeof(TController);

            _logger.ControllerExtracted();
            return controller;
        }

        private bool ValidateMethodConcreteness(
            MethodInfo method,
            Type controller)
        {
            if (controller.IsAbstract)
            {
                _logger.AbstractController();
                return false;
            }

            if (method.DeclaringType != controller)
            {
                _logger.MethodDeclaringType();
                return false;
            }

            return true;
        }

        private bool TryExtractActionDescriptor(
            MethodInfo method,
            ActionDescriptorCollection actionDescriptors,
            [NotNullWhen(true)] out ControllerActionDescriptor? actionDescriptor)
        {
            actionDescriptor = actionDescriptors.Items
                .OfType<ControllerActionDescriptor>()
                .FirstOrDefault(descriptor => descriptor.MethodInfo == method);

            if (actionDescriptor == null)
            {
                _logger.NoActionDescriptorFound(method);
                return false;
            }

            _logger.ActionDescriptorExtracted();
            return true;
        }

        private bool ValidateBoundProperties(ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor.BoundProperties.Any())
            {
                _logger.BoundProperties();
                return false;
            }

            return true;
        }

        private ICollection<KeyValuePair<string, object?>> ExtractRouteValues(
            HttpContext? httpContext,
            ActionDescriptor actionDescriptor,
            ReadOnlyCollection<Expression> arguments,
            UriOptions? options)
        {
            var routeValues = new List<KeyValuePair<string, object?>>();
            var routeValueKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            var parameters = actionDescriptor.Parameters
                .OfType<ControllerParameterDescriptor>();

            foreach (var parameter in parameters)
            {
                ModelMetadata metadata;
                if (_modelMetadataProvider is ModelMetadataProvider modelMetadataProviderBase)
                {
                    // The default model metadata provider derives from ModelMetadataProvider
                    // and can therefore supply information about attributes applied to parameters.
                    metadata = modelMetadataProviderBase.GetMetadataForParameter(parameter.ParameterInfo);
                }
                else
                {
                    // For backward compatibility, if there's a custom model metadata provider that
                    // only implements the older IModelMetadataProvider interface, access the more
                    // limited metadata information it supplies.
                    metadata = _modelMetadataProvider.GetMetadataForType(parameter.ParameterType);
                }

                var bindingInfo = parameter.BindingInfo ?? new BindingInfo();
                bindingInfo.TryApplyBindingInfo(metadata);

                if (!metadata.IsBindingAllowed)
                {
                    _logger.BindingNotAllowed(parameter.Name);
                    continue;
                }

                string key = bindingInfo.BinderModelName ?? metadata.BinderModelName ?? parameter.Name;
                var bindingSource = bindingInfo.BindingSource ?? metadata.BindingSource;

                var bindingSourceFilter = _globalOptions.BindingSourceFilter
                    ?? BindingSourceFilterDelegate;

                if (!bindingSourceFilter(bindingSource))
                {
                    _logger.DisallowedBindingSource(parameter.Name, bindingSource);
                    continue;
                }

                var argument = arguments[parameter.ParameterInfo.Position];

                object? value;

                if (argument is ConstantExpression ce)
                {
                    value = ce.Value;
                }
                else
                {
                    value = EvaluateExpression(argument, options);
                }

                routeValues.Add(new KeyValuePair<string, object?>(key, value));
                routeValueKeys.Add(key);
                _logger.RouteValueExtracted(key, value);
            }

            NormalizeRouteValues(
                httpContext,
                actionDescriptor,
                routeValues,
                routeValueKeys);

            return routeValues;
        }

        private static bool BindingSourceFilter(BindingSource? bindingSource)
        {
            return bindingSource == null // Might be null in apps that don't use InferParameterBindingInfoConvention.
                || bindingSource.CanAcceptDataFrom(BindingSource.Query)
                || bindingSource.CanAcceptDataFrom(BindingSource.Path);
        }

        private object? EvaluateExpression(
            Expression expression,
            UriOptions? options)
        {
            bool? bypassCachedExpressionCompiler = options?.BypassCachedExpressionCompiler
                ?? _globalOptions.BypassCachedExpressionCompiler;

            if (bypassCachedExpressionCompiler is not true)
            {
                return CachedExpressionCompiler.Evaluate(expression);
            }
            else
            {
                var converted = Expression.Convert(expression, typeof(object));
                var lambda = Expression.Lambda<Func<object?, object?>>(
                    converted,
                    FakeParameter);
                var func = lambda.Compile();

                return func(null);
            }
        }

        private void NormalizeRouteValues(
            HttpContext? httpContext,
            ActionDescriptor actionDescriptor,
            ICollection<KeyValuePair<string, object?>> routeValues,
            HashSet<string> routeValueKeys)
        {
            var requiredValues = GetRequiredValues(actionDescriptor);

            foreach (var kv in requiredValues)
            {
                if (!routeValueKeys.Contains(kv.Key))
                {
                    routeValues.Add(new KeyValuePair<string, object?>(kv.Key, kv.Value));
                    routeValueKeys.Add(kv.Key);
                    _logger.RouteValueExtracted(kv.Key, kv.Value);
                }
            }

            var ambientValues = GetAmbientValues(httpContext);

            if (ambientValues != null)
            {
                foreach (var kv in ambientValues)
                {
                    if (!routeValueKeys.Contains(kv.Key))
                    {
                        routeValues.Add(new KeyValuePair<string, object?>(kv.Key, null));
                        routeValueKeys.Add(kv.Key);
                        _logger.RouteValueExtracted(kv.Key, null);
                    }
                }
            }
        }

        private static IDictionary<string, string?> GetRequiredValues(
            ActionDescriptor actionDescriptor)
        {
            return actionDescriptor.RouteValues;
        }

        private static RouteValueDictionary? GetAmbientValues(
            HttpContext? httpContext)
        {
            return httpContext?.Features.Get<IRouteValuesFeature>()?.RouteValues;
        }
    }
}
