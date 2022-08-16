using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace UriGeneration.Internal
{
    internal static partial class LoggerExtensions
    {
        [LoggerMessage(1, LogLevel.Debug, "Succesfully extracted MethodCallExpression.", EventName = nameof(MethodCallExtracted))]
        public static partial void MethodCallExtracted(this ILogger logger);

        [LoggerMessage(2, LogLevel.Debug, "Failed to extract MethodCallExpression from expression's body: {ExpressionBody} must be a MethodCallExpression.", EventName = nameof(MethodCallNotExtracted))]
        public static partial void MethodCallNotExtracted(this ILogger logger, Expression expressionBody);

        [LoggerMessage(3, LogLevel.Debug, "Succesfully extracted MethodInfo.", EventName = nameof(MethodExtracted))]
        public static partial void MethodExtracted(this ILogger logger);

        [LoggerMessage(4, LogLevel.Debug, "Failed to extract MethodInfo from MethodCallExpression: {MethodCallExpression} must point to the method which isn't null.", EventName = nameof(MethodNotExtracted))]
        public static partial void MethodNotExtracted(this ILogger logger, MethodCallExpression methodCallExpression);

        [LoggerMessage(5, LogLevel.Debug, "Succesfully extracted Type of TController.", EventName = nameof(ControllerExtracted))]
        public static partial void ControllerExtracted(this ILogger logger);

        [LoggerMessage(6, LogLevel.Debug, "Failed to extract Type from TController: typeof(TController) cannot be null.", EventName = nameof(ControllerNotExtracted))]
        public static partial void ControllerNotExtracted(this ILogger logger);

        [LoggerMessage(7, LogLevel.Debug, "TController cannot be abstract.", EventName = nameof(AbstractController))]
        public static partial void AbstractController(this ILogger logger);
        
        [LoggerMessage(8, LogLevel.Debug, "Expression must point to the method with the same declaring type as TController.", EventName = nameof(OverridenMethod))]
        public static partial void OverridenMethod(this ILogger logger);
        
        [LoggerMessage(9, LogLevel.Debug, "Expression must point to the method which has {EndpointName} endpoint name specified.", EventName = nameof(EndpointNameNotFound))]
        public static partial void EndpointNameNotFound(this ILogger logger, string endpointName);

        [LoggerMessage(10, LogLevel.Debug, "Succesfuly extracted method's name: {MethodName}.", EventName = nameof(MethodNameExtracted))]
        public static partial void MethodNameExtracted(this ILogger logger, string methodName);
        
        [LoggerMessage(11, LogLevel.Debug, "{MethodName} cannot have NonActionAttribute specified.", EventName = nameof(MethodNameNonActionAttribute))]
        public static partial void MethodNameNonActionAttribute(this ILogger logger, string methodName);

        [LoggerMessage(12, LogLevel.Debug, "Succesfuly extracted controller's name: {ControllerName}.", EventName = nameof(ControllerNameExtracted))]
        public static partial void ControllerNameExtracted(this ILogger logger, string controllerName);
        
        [LoggerMessage(13, LogLevel.Debug, "TController cannot have NonControllerAttribute specified.", EventName = nameof(ControllerNameNonControllerAttribute))]
        public static partial void ControllerNameNonControllerAttribute(this ILogger logger);

        [LoggerMessage(14, LogLevel.Debug, "Succesfully extracted route value: {Key}, {Value}.", EventName = nameof(RouteValueExtracted))]
        public static partial void RouteValueExtracted(this ILogger logger, string? key, object? value);

        [LoggerMessage(15, LogLevel.Debug, "Succesfully extracted all values from expression.", EventName = nameof(ValuesExtracted))]
        public static partial void ValuesExtracted(this ILogger logger);

        [LoggerMessage(16, LogLevel.Debug, "Failed to extract any values from expression {Expression}: an exception occured.", EventName = nameof(ValuesException))]
        public static partial void ValuesException(this ILogger logger, LambdaExpression expression);
    }
}
