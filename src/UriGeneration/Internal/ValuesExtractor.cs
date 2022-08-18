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

        private static readonly ParameterExpression UnusedParameterExpr =
            Expression.Parameter(typeof(object), "_unused");

        private readonly IMethodCacheAccessor _methodCacheAccessor;
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

            _methodCacheAccessor = methodCacheAccessor;
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
                if (!TryExtractMethodCall(action.Body, out var methodCall)
                    || !TryExtractMethod(methodCall, out var method)
                    || !TryExtractController<TController>(out var controller))
                {
                    return false;
                }

                var methodCache = _methodCacheAccessor.Cache;
                var key = (method, controller, endpointName);

                if (options?.BypassMethodCache is not true
                    && methodCache.TryGetValue(key, out MethodCacheEntry entry))
                {
                    if (entry.IsValid)
                    {
                        _logger.ValidCacheEntryRetrieved(
                            entry.MethodName,
                            entry.ControllerName,
                            AreaKey,
                            entry.ControllerAreaName);

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
                        methodCache.Set(key, invalidEntry, CacheEntryOptions);
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
                    methodCache.Set(key, validEntry, CacheEntryOptions);
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
                _logger.ValuesNotExtracted(action, exception);
                return false;
            }
        }

        private bool TryExtractMethodCall(
            Expression actionBody,
            out MethodCallExpression methodCall)
        {
            methodCall = (actionBody as MethodCallExpression)!;

            if (methodCall == null
                && actionBody is UnaryExpression objectCast)
            {
                methodCall = (objectCast.Operand as MethodCallExpression)!;
            }

            if (methodCall == null)
            {
                _logger.MethodCallNotExtractedCast(actionBody);
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
                _logger.MethodNotExtractedNull(methodCall);
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
                _logger.ControllerNotExtractedNull();
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
                _logger.MethodNameNotExtractedNonAction(methodName);
                return false;
            }

            methodName = methodName.RemoveSuffix(AsyncSuffix);

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
                _logger.ControllerNameNotExtractedNonController(controllerName);
                return false;
            }

            controllerName = controllerName.RemoveSuffix(ControllerSuffix);

            _logger.ControllerNameExtracted(controllerName);
            return true;
        }

        private string? ExtractControllerAreaName(Type controller)
        {
            var areaAttribute = controller
                .GetCustomAttributes<AreaAttribute>(inherit: true)
                .FirstOrDefault();

            if (areaAttribute != null)
            {
                string controllerAreaName = areaAttribute.RouteValue;
                _logger.RouteValueExtracted(AreaKey, controllerAreaName);
                return controllerAreaName;
            }

            return default;
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

                if (methodParameters[i].Name != null)
                {
                    routeValues.Add(methodParameters[i].Name, value);
                    _logger.RouteValueExtracted(methodParameters[i].Name, value);
                }
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
                Expression<Func<object, object>> lambdaExpr =
                    Expression.Lambda<Func<object, object>>(
                        Expression.Convert(expression, typeof(object)),
                        UnusedParameterExpr);

                var func = lambdaExpr.Compile();
                return func(null!);
            }
        }
    }
}
