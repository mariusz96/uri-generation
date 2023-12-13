using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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
        private const string AreaKey = "area";

        private static readonly MemoryCacheEntryOptions CacheEntryOptions =
            new() { Size = 1 };

        private static readonly ParameterExpression UnusedParameterExpression =
            Expression.Parameter(typeof(object), "unused");

        private readonly IMethodCacheAccessor _methodCacheAccessor;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorsProvider;
        private readonly ILogger<ValuesExtractor> _logger;

        public ValuesExtractor(
            IMethodCacheAccessor methodCacheAccessor,
            IActionDescriptorCollectionProvider actionDescriptorsProvider,
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

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _methodCacheAccessor = methodCacheAccessor;
            _actionDescriptorsProvider = actionDescriptorsProvider;
            _logger = logger;
        }

        public bool TryExtractValues<TController>(
            LambdaExpression action,
            [NotNullWhen(true)] out Values? values,
            UriOptions? options = null)
                where TController : class
        {
            try
            {
                values = default;

                if (!TryExtractMethodCall(action.Body, out var methodCall))
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

                if (options?.BypassMethodCache is not true
                    && methodCache.TryGetValue(key, out MethodCacheEntry? entry))
                {
                    if (entry!.IsValid)
                    {
                        _logger.ValidCacheEntryRetrieved(entry.ActionDescriptor);

                        var entryActionDescriptor = entry.ActionDescriptor;

                        var entryRouteValues = ExtractRouteValues(
                            entryActionDescriptor,
                            methodCall.Arguments,
                            options);

                        _logger.ValuesExtracted();

                        values = new Values(
                            entryActionDescriptor.ActionName,
                            entryActionDescriptor.ControllerName,
                            entryRouteValues);
                        return true;
                    }
                    else
                    {
                        _logger.InvalidCacheEntryRetrieved();
                        return false;
                    }
                }

                if (!ValidateMethodConcreteness(method, controller)
                    || !TryExtractActionDescriptor(method, actionDescriptors, out var actionDescriptor))
                {
                    if (options?.BypassMethodCache is not true)
                    {
                        var invalidEntry = MethodCacheEntry.Invalid();
                        methodCache.Set(key, invalidEntry, CacheEntryOptions);
                    }

                    return false;
                }

                var routeValues = ExtractRouteValues(
                    actionDescriptor,
                    methodCall.Arguments,
                    options);

                if (options?.BypassMethodCache is not true)
                {
                    var validEntry = MethodCacheEntry.Valid(actionDescriptor);
                    methodCache.Set(key, validEntry, CacheEntryOptions);
                }

                _logger.ValuesExtracted();

                values = new Values(
                    actionDescriptor.ActionName,
                    actionDescriptor.ControllerName,
                    routeValues);
                return true;
            }
            catch (Exception exception)
            {
                _logger.ValuesNotExtracted(action, exception);

                values = default;
                return false;
            }
        }

        private bool TryExtractMethodCall(
            Expression actionBody,
            [NotNullWhen(true)] out MethodCallExpression? methodCall)
        {
            methodCall = actionBody as MethodCallExpression;

            if (methodCall == null
                && actionBody is UnaryExpression objectCast)
            {
                methodCall = objectCast.Operand as MethodCallExpression;
            }

            if (methodCall == null)
            {
                _logger.MethodCallNotExtracted(actionBody);
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

            if (actionDescriptor is null)
            {
                _logger.NoActionDescriptorFound(method);
                return false;
            }

            _logger.ActionDescriptorExtracted();
            return true;
        }

        private RouteValueDictionary ExtractRouteValues(
            ActionDescriptor actionDescriptor,
            ReadOnlyCollection<Expression> methodCallArguments,
            UriOptions? options)
        {
            var routeValues = new RouteValueDictionary();
            var parameters = actionDescriptor.Parameters
                .OfType<ControllerParameterDescriptor>();

            foreach (var parameter in parameters)
            {
                var bindingSource = parameter.BindingInfo?.BindingSource;

                if (bindingSource is not null // Might be null in controllers with views.
                    && !bindingSource.CanAcceptDataFrom(BindingSource.Query)
                    && !bindingSource.CanAcceptDataFrom(BindingSource.Path))
                {
                    _logger.BindingSource(bindingSource?.Id);
                    continue;
                }

                string key = parameter.Name;

                var methodCallArgument = methodCallArguments[
                    parameter.ParameterInfo.Position];

                object? value;

                if (methodCallArgument is ConstantExpression ce)
                {
                    value = ce.Value;
                }
                else
                {
                    value = EvaluateExpression(methodCallArgument, options);
                }

                routeValues.Add(key, value);
                _logger.RouteValueExtracted(key, value);
            }

            string areaName = ExtractAreaName(actionDescriptor);
            routeValues.Add(AreaKey, areaName);
            _logger.RouteValueExtracted(AreaKey, areaName);

            return routeValues;
        }

        private static object? EvaluateExpression(
            Expression expression,
            UriOptions? options)
        {
            if (options?.BypassCachedExpressionCompiler is not true)
            {
                return CachedExpressionCompiler.Evaluate(expression);
            }
            else
            {
                // See: CachedExpressionCompiler.Evaluate.
                Expression<Func<object?, object?>> lambdaExpression =
                    Expression.Lambda<Func<object?, object?>>(
                        Expression.Convert(expression, typeof(object)),
                        UnusedParameterExpression);

                var func = lambdaExpression.Compile();
                return func(null);
            }
        }

        private static string ExtractAreaName(ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor.RouteValues.TryGetValue(AreaKey, out string? areaName))
            {
                return areaName ?? string.Empty;
            }
            else
            {
                return string.Empty; // Don't use the 'ambient' value of area.
            }
        }
    }
}
