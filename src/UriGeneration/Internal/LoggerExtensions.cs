using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace UriGeneration.Internal
{
    internal static partial class LoggerExtensions
    {
        [LoggerMessage(1001, LogLevel.Debug, "Succesfully extracted MethodCallExpression.", EventName = nameof(MethodCallExtracted))]
        public static partial void MethodCallExtracted(this ILogger logger);

        [LoggerMessage(1002, LogLevel.Debug, "Failed to extract MethodCallExpression from expression's body: {ExpressionBody} must be a MethodCallExpression.", EventName = nameof(MethodCallNotExtracted))]
        public static partial void MethodCallNotExtracted(this ILogger logger, Expression expressionBody);

        [LoggerMessage(1003, LogLevel.Debug, "Succesfully extracted MethodInfo.", EventName = nameof(MethodExtracted))]
        public static partial void MethodExtracted(this ILogger logger);

        [LoggerMessage(1004, LogLevel.Debug, "Failed to extract MethodInfo from MethodCallExpression: {MethodCallExpression} must point to the method which isn't null.", EventName = nameof(MethodNotExtracted))]
        public static partial void MethodNotExtracted(this ILogger logger, MethodCallExpression methodCallExpression);

        [LoggerMessage(1005, LogLevel.Debug, "Succesfully extracted Type of TController.", EventName = nameof(ControllerExtracted))]
        public static partial void ControllerExtracted(this ILogger logger);

        [LoggerMessage(1006, LogLevel.Debug, "Failed to extract Type of TController: typeof(TController) cannot be null.", EventName = nameof(ControllerNotExtracted))]
        public static partial void ControllerNotExtracted(this ILogger logger);

        [LoggerMessage(1007, LogLevel.Debug, "Succesfully retrieved valid entry for a given cache key.", EventName = nameof(ValidCacheEntryRetrieved))]
        public static partial void ValidCacheEntryRetrieved(this ILogger logger);

        [LoggerMessage(1008, LogLevel.Debug, "Succesfully retrieved invalid entry for a given cache key.", EventName = nameof(InvalidCacheEntryRetrieved))]
        public static partial void InvalidCacheEntryRetrieved(this ILogger logger);

        [LoggerMessage(1009, LogLevel.Debug, "TController cannot be abstract.", EventName = nameof(AbstractController))]
        public static partial void AbstractController(this ILogger logger);
        
        [LoggerMessage(1010, LogLevel.Debug, "Expression must point to the method with the same declaring type as TController.", EventName = nameof(OverridenMethod))]
        public static partial void OverridenMethod(this ILogger logger);
        
        [LoggerMessage(1011, LogLevel.Debug, "Expression must point to the method which has {EndpointName} endpoint name specified.", EventName = nameof(EndpointNameNotFound))]
        public static partial void EndpointNameNotFound(this ILogger logger, string endpointName);

        [LoggerMessage(1012, LogLevel.Debug, "Succesfuly extracted method's name: {MethodName}.", EventName = nameof(MethodNameExtracted))]
        public static partial void MethodNameExtracted(this ILogger logger, string methodName);
        
        [LoggerMessage(1013, LogLevel.Debug, "{MethodName} cannot have NonActionAttribute specified.", EventName = nameof(MethodNameNonActionAttribute))]
        public static partial void MethodNameNonActionAttribute(this ILogger logger, string methodName);

        [LoggerMessage(1014, LogLevel.Debug, "Succesfuly extracted controller's name: {ControllerName}.", EventName = nameof(ControllerNameExtracted))]
        public static partial void ControllerNameExtracted(this ILogger logger, string controllerName);
        
        [LoggerMessage(1015, LogLevel.Debug, "TController cannot have NonControllerAttribute specified.", EventName = nameof(ControllerNameNonControllerAttribute))]
        public static partial void ControllerNameNonControllerAttribute(this ILogger logger);

        [LoggerMessage(1016, LogLevel.Debug, "Succesfully extracted route value: {Key}, {Value}.", EventName = nameof(RouteValueExtracted))]
        public static partial void RouteValueExtracted(this ILogger logger, string? key, object? value);

        [LoggerMessage(1017, LogLevel.Debug, "Succesfully extracted values from expression.", EventName = nameof(ValuesExtracted))]
        public static partial void ValuesExtracted(this ILogger logger);

        [LoggerMessage(1018, LogLevel.Debug, "Failed to extract values from expression {Expression}: an exception occured.", EventName = nameof(ValuesException))]
        public static partial void ValuesException(this ILogger logger, LambdaExpression expression, Exception exception);
    }
}
