using Microsoft.Extensions.Logging;
using System.Linq.Expressions;

namespace UriGeneration.Internal
{
    internal static partial class LoggerExtensions
    {
        [LoggerMessage(1001, LogLevel.Debug, "Successfully extracted MethodCallExpression.", EventName = nameof(MethodCallExtracted))]
        public static partial void MethodCallExtracted(this ILogger logger);

        [LoggerMessage(1002, LogLevel.Debug, "Failed to extract MethodCallExpression from expression's body: {ExpressionBody} must be a MethodCallExpression.", EventName = nameof(MethodCallNotExtractedCast))]
        public static partial void MethodCallNotExtractedCast(this ILogger logger, Expression expressionBody);

        [LoggerMessage(1003, LogLevel.Debug, "Successfully extracted MethodInfo.", EventName = nameof(MethodExtracted))]
        public static partial void MethodExtracted(this ILogger logger);

        [LoggerMessage(1004, LogLevel.Debug, "Failed to extract MethodInfo from MethodCallExpression: {MethodCallExpression} must point to the method which isn't null.", EventName = nameof(MethodNotExtractedNull))]
        public static partial void MethodNotExtractedNull(this ILogger logger, MethodCallExpression methodCallExpression);

        [LoggerMessage(1005, LogLevel.Debug, "Successfully extracted Type of TController.", EventName = nameof(ControllerExtracted))]
        public static partial void ControllerExtracted(this ILogger logger);

        [LoggerMessage(1006, LogLevel.Debug, "Failed to extract Type of TController: typeof(TController) cannot be null.", EventName = nameof(ControllerNotExtractedNull))]
        public static partial void ControllerNotExtractedNull(this ILogger logger);

        [LoggerMessage(1007, LogLevel.Debug, "Successfully retrieved valid cache entry with values: {MethodName}, {ControllerName} and {AreaKey}, {ControllerAreaName} (if present).", EventName = nameof(ValidCacheEntryRetrieved))]
        public static partial void ValidCacheEntryRetrieved(this ILogger logger, string methodName, string controllerName, string areaKey, string? controllerAreaName);

        [LoggerMessage(1008, LogLevel.Debug, "Successfully retrieved invalid cache entry: no values will be extracted (see previous log messages for further details).", EventName = nameof(InvalidCacheEntryRetrieved))]
        public static partial void InvalidCacheEntryRetrieved(this ILogger logger);

        [LoggerMessage(1009, LogLevel.Debug, "TController cannot be abstract.", EventName = nameof(AbstractController))]
        public static partial void AbstractController(this ILogger logger);
        
        [LoggerMessage(1010, LogLevel.Debug, "Expression must point to the method with the same declaring type as TController.", EventName = nameof(MethodDeclaringType))]
        public static partial void MethodDeclaringType(this ILogger logger);
        
        [LoggerMessage(1011, LogLevel.Debug, "Expression must point to the method which has {EndpointName} endpoint name specified.", EventName = nameof(EndpointNameNotFound))]
        public static partial void EndpointNameNotFound(this ILogger logger, string endpointName);

        [LoggerMessage(1012, LogLevel.Debug, "Excluding method's parameter with position {MethodParameterPosition}: name cannot be null.", EventName = nameof(MethodParameterExcludedName))]
        public static partial void MethodParameterExcludedName(this ILogger logger, int methodParameterPosition);

        [LoggerMessage(1013, LogLevel.Debug, "Excluding method's parameter: {MethodParameterName} cannot be of Type IFormFile, IEnumerable<IFormFile>, CancellationToken or FormCollection.", EventName = nameof(MethodParameterExcludedType))]
        public static partial void MethodParameterExcludedType(this ILogger logger, string? methodParameterName);

        [LoggerMessage(1014, LogLevel.Debug, "Excluding method's parameter: {MethodParameterName} cannot have FromBody, FromForm, FromHeader or FromServices attribute specified.", EventName = nameof(MethodParameterExcludedAttribute))]
        public static partial void MethodParameterExcludedAttribute(this ILogger logger, string? methodParameterName);

        [LoggerMessage(1015, LogLevel.Debug, "Successfully extracted method's name: {MethodName}.", EventName = nameof(MethodNameExtracted))]
        public static partial void MethodNameExtracted(this ILogger logger, string methodName);
        
        [LoggerMessage(1016, LogLevel.Debug, "{MethodName} cannot have NonActionAttribute specified.", EventName = nameof(MethodNameNotExtractedNonAction))]
        public static partial void MethodNameNotExtractedNonAction(this ILogger logger, string methodName);

        [LoggerMessage(1017, LogLevel.Debug, "Succesfuly extracted controller's name: {ControllerName}.", EventName = nameof(ControllerNameExtracted))]
        public static partial void ControllerNameExtracted(this ILogger logger, string controllerName);
        
        [LoggerMessage(1018, LogLevel.Debug, "{ControllerName} cannot have NonControllerAttribute specified.", EventName = nameof(ControllerNameNotExtractedNonController))]
        public static partial void ControllerNameNotExtractedNonController(this ILogger logger, string controllerName);

        [LoggerMessage(1019, LogLevel.Debug, "Successfully extracted route value: {Key}, {Value}.", EventName = nameof(RouteValueExtracted))]
        public static partial void RouteValueExtracted(this ILogger logger, string? key, object? value);

        [LoggerMessage(1020, LogLevel.Debug, "Successfully extracted all values from expression.", EventName = nameof(ValuesExtracted))]
        public static partial void ValuesExtracted(this ILogger logger);

        [LoggerMessage(1021, LogLevel.Debug, "Failed to extract one or more values from expression {Expression}: an exception occurred.", EventName = nameof(ValuesNotExtracted))]
        public static partial void ValuesNotExtracted(this ILogger logger, LambdaExpression expression, Exception exception);
    }
}
