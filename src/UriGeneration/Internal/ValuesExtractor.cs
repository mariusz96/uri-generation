using UriGeneration.AspNetWebStack;
using UriGeneration.Internal.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.ObjectModel;
using System.Linq.Expressions;
using System.Reflection;

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

        public ValuesExtractor(IMethodCacheAccessor methodCacheAccessor)
        {
            if (methodCacheAccessor == null)
            {
                throw new ArgumentNullException(nameof(methodCacheAccessor));
            }

            _methodCache = methodCacheAccessor.Cache;
        }

        public Values ExtractValues<TController>(
            LambdaExpression action,
            string? endpointName = null,
            UriOptions? options = null)
                where TController : ControllerBase
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            options ??= UriOptions.Default;

            var methodCall = ExtractMethodCall(action);

            var method = ExtractMethod(methodCall);

            var controller = ExtractController<TController>();

            var key = (method, controller, endpointName);

            if (!options.BypassMethodCache
                && _methodCache.TryGetValue(key, out MethodCacheEntry entry))
            {
                var entryRouteValues = ExtractRouteValues(
                    entry.MethodParameters,
                    methodCall.Arguments,
                    entry.ControllerAreaName,
                    options);

                return new Values(
                    entry.MethodName,
                    entry.ControllerName,
                    entryRouteValues);
            }

            ValidateMethodPolymorphism(method, controller);

            ValidateEnpointName(endpointName, method);

            var methodParameters = method.GetParameters();

            string methodName = ExtractMethodName(method);

            string controllerName = ExtractControllerName(controller);

            string? controllerAreaName = ExtractControllerAreaName(controller);

            var routeValues = ExtractRouteValues(
                methodParameters,
                methodCall.Arguments,
                controllerAreaName,
                options);

            if (!options.BypassMethodCache)
            {
                var newEntry = new MethodCacheEntry(
                    methodName,
                    controllerName,
                    methodParameters,
                    controllerAreaName);

                _methodCache.Set(key, newEntry, CacheEntryOptions);
            }

            return new Values(
                methodName,
                controllerName,
                routeValues);
        }

        private static MethodCallExpression ExtractMethodCall(
            LambdaExpression action)
        {
            var methodCall = action.Body as MethodCallExpression;

            if (methodCall == null
                && action.Body is UnaryExpression objectCast)
            {
                methodCall = objectCast.Operand as MethodCallExpression;
            }

            if (methodCall == null)
            {
                throw new InvalidOperationException("Expression's body must " +
                    $"be a {nameof(MethodCallExpression)}.");
            }

            return methodCall;
        }

        private static MethodInfo ExtractMethod(
            MethodCallExpression methodCall)
        {
            var method = methodCall.Method;

            if (method == null)
            {
                throw new InvalidOperationException("Expression must point " +
                    "to the method which isn't null.");
            }

            return method;
        }

        private static Type ExtractController<TController>()
            where TController : ControllerBase
        {
            var controller = typeof(TController);

            if (controller == null)
            {
                throw new InvalidOperationException($"Type of " +
                    $"{nameof(TController)} cannot be null.");
            }

            return controller;
        }

        private static void ValidateMethodPolymorphism(
            MethodInfo method,
            Type controller)
        {
            if (controller.IsInterface
                || controller.IsAbstract)
            {
                throw new InvalidOperationException("TController cannot be " +
                    "abstract.");
            }

            if (method.DeclaringType != controller)
            {
                throw new InvalidOperationException("Expression must point " +
                    "to the method with the same declaring type as " +
                    "TController.");
            }
        }

        private static void ValidateEnpointName(
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
                    throw new InvalidOperationException("Expression must " +
                        $"point to the method which has {endpointName} " +
                        "endpoint name specified.");
                }
            }
        }

        private static string ExtractMethodName(MethodInfo method)
        {
            if (method.IsDefined(typeof(NonActionAttribute), inherit: true))
            {
                throw new InvalidOperationException("Expression must point " +
                    "to the method which doesn't have " +
                    $"{nameof(NonActionAttribute)} specified.");
            }

            string methodName = method.Name;

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
                return actionNameAttribute.Name;
            }

            return methodName;
        }

        private static string ExtractControllerName(Type controller)
        {
            if (controller.IsDefined(
                    typeof(NonControllerAttribute),
                    inherit: true))
            {
                throw new InvalidOperationException("TController cannot have " +
                    $"{nameof(NonControllerAttribute)} specified.");
            }

            string controllerName = controller.Name;

            if (controllerName.EndsWith(ControllerSuffix))
            {
                int index = controllerName.LastIndexOf(ControllerSuffix);
                controllerName = controllerName.Remove(index);
            }

            return controllerName;
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

        private static RouteValueDictionary ExtractRouteValues(
            ParameterInfo[] methodParameters,
            ReadOnlyCollection<Expression> methodCallArguments,
            string? controllerAreaName,
            UriOptions options)
        {
            var routeValues = new RouteValueDictionary { };

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
            }

            if (controllerAreaName != null)
            {
                routeValues.Add(AreaKey, controllerAreaName);
            }

            return routeValues;
        }

        private static object EvaluateExpression(
            Expression expression,
            UriOptions options)
        {
            if (!options.BypassCachedExpressionCompiler)
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

                return func(null);
            }
        }
    }
}
