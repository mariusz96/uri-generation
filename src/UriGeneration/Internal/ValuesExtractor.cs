﻿using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

        private static readonly ParameterExpression FakeParameter =
            Expression.Parameter(typeof(object), null);
        private static readonly MemoryCacheEntryOptions CacheEntryOptions =
            new() { Size = 1 };

        private readonly IMethodCacheAccessor _methodCacheAccessor;
        private readonly IActionDescriptorCollectionProvider _actionDescriptorsProvider;
        private readonly IModelMetadataProvider _modelMetadataProvider;
        private readonly ILogger<ValuesExtractor> _logger;

        public ValuesExtractor(
            IMethodCacheAccessor methodCacheAccessor,
            IActionDescriptorCollectionProvider actionDescriptorsProvider,
            IModelMetadataProvider modelMetadataProvider,
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

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _methodCacheAccessor = methodCacheAccessor;
            _actionDescriptorsProvider = actionDescriptorsProvider;
            _modelMetadataProvider = modelMetadataProvider;
            _logger = logger;
        }

        public bool TryExtractValues<TController>(
            LambdaExpression expression,
            [NotNullWhen(true)] out Values? values,
            UriOptions? options = null)
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

                if (options?.BypassMethodCache is not true
                    && methodCache.TryGetValue(key, out MethodCacheEntry? entry))
                {
                    if (entry!.IsValid)
                    {
                        _logger.ValidCacheEntryRetrieved(entry.ActionDescriptor);

                        var entryRouteValues = ExtractRouteValues(
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
                    if (options?.BypassMethodCache is not true)
                    {
                        var invalidEntry = MethodCacheEntry.Invalid();
                        methodCache.Set(key, invalidEntry, CacheEntryOptions);
                    }

                    return false;
                }

                var routeValues = ExtractRouteValues(
                    descriptor,
                    methodCall.Arguments,
                    options);

                if (options?.BypassMethodCache is not true)
                {
                    var validEntry = MethodCacheEntry.Valid(descriptor);
                    methodCache.Set(key, validEntry, CacheEntryOptions);
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

        private ICollection<KeyValuePair<string, object?>> ExtractRouteValues(
            ActionDescriptor actionDescriptor,
            ReadOnlyCollection<Expression> methodCallArguments,
            UriOptions? options)
        {
            var routeValues = new List<KeyValuePair<string, object?>>();
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
                    // limited metadata information it supplies. In this scenario, validation attributes
                    // are not supported on parameters.
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

                if (bindingSource == null // Might be null in apps that don't use InferParameterBindingInfoConvention.
                    || bindingSource.CanAcceptDataFrom(BindingSource.Query)
                    || bindingSource.CanAcceptDataFrom(BindingSource.Path))
                {
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

                    routeValues.Add(new KeyValuePair<string, object?>(key, value));
                    _logger.RouteValueExtracted(key, value);
                }
                else
                {
                    _logger.DisallowedBindingSource(parameter.Name, bindingSource?.Id);
                    continue;
                }
            }

            string areaName = ExtractAreaName(actionDescriptor);

            routeValues.Add(new KeyValuePair<string, object?>(AreaKey, areaName));
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
                var converted = Expression.Convert(expression, typeof(object));
                var lambda = Expression.Lambda<Func<object?, object?>>(
                    converted,
                    FakeParameter);
                var func = lambda.Compile();

                return func(null);
            }
        }

        private static string ExtractAreaName(ActionDescriptor actionDescriptor)
        {
            if (actionDescriptor.RouteValues.TryGetValue(AreaKey, out string? areaName)
                && areaName != null)
            {
                return areaName;
            }
            else
            {
                return string.Empty; // Don't use the 'ambient' value of area.
            }
        }
    }
}
