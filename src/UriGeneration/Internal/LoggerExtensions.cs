using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System.Linq.Expressions;
using System.Reflection;

namespace UriGeneration.Internal
{
    internal static partial class LoggerExtensions
    {
        [LoggerMessage(1001, LogLevel.Debug, "Successfully extracted MethodCallExpression.", EventName = nameof(MethodCallExtracted))]
        public static partial void MethodCallExtracted(this ILogger logger);

        [LoggerMessage(1002, LogLevel.Debug, "Failed to extract MethodCallExpression from expression's body: {ExpressionBody} must be a MethodCallExpression.", EventName = nameof(MethodCallNotExtracted))]
        public static partial void MethodCallNotExtracted(this ILogger logger, Expression expressionBody);

        [LoggerMessage(1003, LogLevel.Debug, "Successfully extracted MethodInfo.", EventName = nameof(MethodExtracted))]
        public static partial void MethodExtracted(this ILogger logger);

        [LoggerMessage(1004, LogLevel.Debug, "Successfully extracted Type of TController.", EventName = nameof(ControllerExtracted))]
        public static partial void ControllerExtracted(this ILogger logger);

        [LoggerMessage(1005, LogLevel.Debug, "Successfully retrieved valid cache entry with value: {ActionDescriptor}.", EventName = nameof(ValidCacheEntryRetrieved))]
        public static partial void ValidCacheEntryRetrieved(this ILogger logger, ActionDescriptor actionDescriptor);

        [LoggerMessage(1006, LogLevel.Debug, "Successfully retrieved invalid cache entry: no values will be extracted (see previous log messages for further details).", EventName = nameof(InvalidCacheEntryRetrieved))]
        public static partial void InvalidCacheEntryRetrieved(this ILogger logger);

        [LoggerMessage(1007, LogLevel.Debug, "TController cannot be abstract.", EventName = nameof(AbstractController))]
        public static partial void AbstractController(this ILogger logger);
        
        [LoggerMessage(1008, LogLevel.Debug, "Expression must point to the method with the same declaring type as TController.", EventName = nameof(MethodDeclaringType))]
        public static partial void MethodDeclaringType(this ILogger logger);

        [LoggerMessage(1009, LogLevel.Debug, "Successfully extracted MethodInfo's ActionDescriptor.", EventName = nameof(ActionDescriptorExtracted))]
        public static partial void ActionDescriptorExtracted(this ILogger logger);

        [LoggerMessage(1010, LogLevel.Debug, "Controller cannot have bound properties.", EventName = nameof(BoundProperties))]
        public static partial void BoundProperties(this ILogger logger);

        [LoggerMessage(1011, LogLevel.Debug, "No ActionDescriptor with MethodInfo: {MethodInfo} found in ActionDescriptorCollection.", EventName = nameof(NoActionDescriptorFound))]
        public static partial void NoActionDescriptorFound(this ILogger logger, MethodInfo methodInfo);

        [LoggerMessage(1012, LogLevel.Debug, "Binding not allowed for parameter: {ParameterName}.", EventName = nameof(BindingNotAllowed))]
        public static partial void BindingNotAllowed(this ILogger logger, string parameterName);

        [LoggerMessage(1013, LogLevel.Debug, "Parameter: {ParameterName} cannot have binding source: {BindingSource}.", EventName = nameof(DisallowedBindingSource))]
        public static partial void DisallowedBindingSource(this ILogger logger, string parameterName, BindingSource? bindingSource);

        [LoggerMessage(1014, LogLevel.Debug, "Successfully extracted route value: {Key}, {Value}.", EventName = nameof(RouteValueExtracted))]
        public static partial void RouteValueExtracted(this ILogger logger, string? key, object? value);

        [LoggerMessage(1015, LogLevel.Debug, "Successfully extracted all values from expression.", EventName = nameof(ValuesExtracted))]
        public static partial void ValuesExtracted(this ILogger logger);

        [LoggerMessage(1016, LogLevel.Debug, "Failed to extract one or more values from expression {Expression}: an exception occurred.", EventName = nameof(ValuesNotExtracted))]
        public static partial void ValuesNotExtracted(this ILogger logger, LambdaExpression expression, Exception exception);
    }
}
