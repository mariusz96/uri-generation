using UriGeneration.AspNetWebStack;
using UriGeneration.Internal.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace UriGeneration.Internal
{
    internal class ValuesExtractor : IValuesExtractor
    {
        private const string ControllerSuffix = "Controller";
        private const string AsyncSuffix = "Async";
        private const string AreaKey = "area";

        private static readonly MemoryCacheEntryOptions CacheEntryOptions =
            new() { Size = 1 };

        private readonly IMemoryCache _methodCache;
        private readonly ILogger<ValuesExtractor> _logger;

        public ValuesExtractor(
            IMethodCacheAccessor methodCacheAccessor,
            ILogger<ValuesExtractor> logger)
        {
            if (methodCacheAccessor == null)
            {
                throw new ArgumentNullException(nameof(methodCacheAccessor));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            _methodCache = methodCacheAccessor.Cache;
            _logger = logger;
        }

        public bool TryExtractValues<TController>(
            LambdaExpression action,
            out Values values,
            string? endpointName = null,
            UriOptions? options = null)
                where TController : ControllerBase
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            values = default!;

            try
            {
                if (!TryExtractMethodCall(action, out var methodCall)
                    || !TryExtractMethod(methodCall, out var method)
                    || !TryExtractController<TController>(out var controller))
                {
                    return false;
                }

                var key = (method, controller, endpointName);

                if (options?.BypassMethodCache is not true
                    && _methodCache.TryGetValue(key, out MethodCacheEntry entry))
                {
                    if (entry.IsValid)
                    {
                        _logger.ValidCacheEntryRetrieved();

                        var entryRouteValues = ExtractRouteValues(
                            entry.MethodParameters,
                            methodCall.Arguments,
                            entry.ControllerAreaName,
                            options);

                        values = new Values(
                            entry.MethodName,
                            entry.ControllerName,
                            entryRouteValues);
                        return true;
                    }
                    else
                    {
                        _logger.InvalidCacheEntryRetrieved();
                        return false;
                    }
                }

                if (!ValidateMethodPolymorphism(method, controller)
                    || !ValidateEnpointName(endpointName, method))
                {
                    if (options?.BypassMethodCache is not true)
                    {
                        var invalidEntry = MethodCacheEntry.Invalid();
                        _methodCache.Set(key, invalidEntry, CacheEntryOptions);
                    }

                    return false;
                }

                var methodParameters = method.GetParameters();

                if (!TryExtractMethodName(method, out var methodName)
                    || !TryExtractControllerName(controller, out var controllerName))
                {
                    return false;
                }

                string? controllerAreaName = ExtractControllerAreaName(
                    controller);

                var routeValues = ExtractRouteValues(
                    methodParameters,
                    methodCall.Arguments,
                    controllerAreaName,
                    options);

                if (options?.BypassMethodCache is not true)
                {
                    var validEntry = MethodCacheEntry.Valid(
                        methodName,
                        controllerName,
                        methodParameters,
                        controllerAreaName);
                    _methodCache.Set(key, validEntry, CacheEntryOptions);
                }

                _logger.ValuesExtracted();

                values = new Values(
                    methodName,
                    controllerName,
                    routeValues);
                return true;
            }
            catch (Exception exception)
            {
                _logger.ValuesException(action, exception);
                return false;
            }
        }

        private bool TryExtractMethodCall(
            LambdaExpression action,
            out MethodCallExpression methodCall)
        {
            methodCall = (action.Body as MethodCallExpression)!;

            if (methodCall == null
                && action.Body is UnaryExpression objectCast)
            {
                methodCall = (objectCast.Operand as MethodCallExpression)!;
            }

            if (methodCall == null)
            {
                _logger.MethodCallNotExtracted(action.Body);
                return false;
            }

            _logger.MethodCallExtracted();
            return true;
        }

        private bool TryExtractMethod(
            MethodCallExpression methodCall,
            out MethodInfo method)
        {
            method = methodCall.Method;

            if (method == null)
            {
                _logger.MethodNotExtracted(methodCall);
                return false;
            }

            _logger.MethodExtracted();
            return true;
        }

        private bool TryExtractController<TController>(out Type controller)
            where TController : ControllerBase
        {
            controller = typeof(TController);

            if (controller == null)
            {
                _logger.ControllerNotExtracted();
                return false;
            }

            _logger.ControllerExtracted();
            return true;
        }

        private bool ValidateMethodPolymorphism(
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
                _logger.OverridenMethod();
                return false;
            }

            return true;
        }

        private bool ValidateEnpointName(
            string? endpointName,
            MethodInfo method)
        {
            if (endpointName != null)
            {
                bool endpointNameDefined = method
                    .GetCustomAttributes(
                        typeof(IRouteTemplateProvider),
                        inherit: true)
                    .Cast<IRouteTemplateProvider>()
                    .Any(a => a.Name == endpointName);

                if (!endpointNameDefined)
                {
                    _logger.EndpointNameNotFound(endpointName);
                    return false;
                }
            }

            return true;
        }

        private bool TryExtractMethodName(
            MethodInfo method,
            out string methodName)
        {
            methodName = method.Name;

            if (method.IsDefined(typeof(NonActionAttribute), inherit: true))
            {
                _logger.MethodNameNonActionAttribute(method.Name);
                return false;
            }

            if (methodName.EndsWith(AsyncSuffix))
            {
                int index = methodName.LastIndexOf(AsyncSuffix);
                methodName = methodName.Remove(index);
            }

            var actionNameAttribute = method
                .GetCustomAttributes<ActionNameAttribute>(inherit: true)
                .FirstOrDefault();

            if (actionNameAttribute != null)
            {
                methodName = actionNameAttribute.Name;
            }

            _logger.MethodNameExtracted(methodName);
            return true;
        }

        private bool TryExtractControllerName(
            Type controller,
            out string controllerName)
        {
            controllerName = controller.Name;

            if (controller.IsDefined(
                    typeof(NonControllerAttribute),
                    inherit: true))
            {
                _logger.ControllerNameNonControllerAttribute();
                return false;
            }

            if (controllerName.EndsWith(ControllerSuffix))
            {
                int index = controllerName.LastIndexOf(ControllerSuffix);
                controllerName = controllerName.Remove(index);
            }

            _logger.ControllerNameExtracted(controllerName);
            return true;
        }

        private static string? ExtractControllerAreaName(Type controller)
        {
            var areaAttribute = controller
                .GetCustomAttributes<AreaAttribute>(inherit: true)
                .FirstOrDefault();

            if (areaAttribute != null)
            {
                return areaAttribute.RouteValue;
            }

            return null;
        }

        private RouteValueDictionary ExtractRouteValues(
            ParameterInfo[] methodParameters,
            ReadOnlyCollection<Expression> methodCallArguments,
            string? controllerAreaName,
            UriOptions? options)
        {
            var routeValues = new RouteValueDictionary();

            for (int i = 0; i < methodParameters.Length; i++)
            {
                var methodCallArgument = methodCallArguments[i];
                object? value;

                if (methodCallArgument is ConstantExpression ce)
                {
                    value = ce.Value;
                }
                else
                {
                    value = EvaluateExpression(methodCallArgument, options);
                }

                routeValues.Add(methodParameters[i].Name, value);
                _logger.RouteValueExtracted(methodParameters[i].Name, value);
            }

            if (controllerAreaName != null)
            {
                routeValues.Add(AreaKey, controllerAreaName);
            }

            return routeValues;
        }

        private static object EvaluateExpression(
            Expression expression,
            UriOptions? options)
        {
            if (options?.BypassCachedExpressionCompiler is not true)
            {
                return CachedExpressionCompiler.Evaluate(expression);
            }
            else
            {
                // see CachedExpressionCompiler.Evaluate
                var unusedParameterExpr = Expression.Parameter(
                    typeof(object),
                    "_unused");
                Expression<Func<object, object>> lambdaExpr =
                    Expression.Lambda<Func<object, object>>(
                        Expression.Convert(expression, typeof(object)),
                        unusedParameterExpr);

                var func = lambdaExpr.Compile();
                return func(null!);
            }
        }
    }
}
