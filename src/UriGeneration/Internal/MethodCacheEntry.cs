using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace UriGeneration.Internal
{
    internal class MethodCacheEntry
    {
        private static readonly MethodCacheEntry InvalidInstance = new(false);

        [MemberNotNull(
            nameof(MethodName),
            nameof(ControllerName),
            nameof(MethodParameters))]
        public bool IsValid { get; }
        public string? MethodName { get; }
        public string? ControllerName { get; }
        public ParameterInfo[]? MethodParameters { get; }
        public string? ControllerAreaName { get; }

        private MethodCacheEntry(
            bool isValid,
            string? methodName = default,
            string? controllerName = default,
            ParameterInfo[]? methodParameters = default,
            string? controllerAreaName = default)
        {
            IsValid = isValid;
            MethodName = methodName;
            ControllerName = controllerName;
            MethodParameters = methodParameters;
            ControllerAreaName = controllerAreaName;
        }

        public static MethodCacheEntry Valid(
            string methodName,
            string controllerName,
            ParameterInfo[] methodParameters,
            string? controllerAreaName = null)
        {
            return new(true,
                methodName,
                controllerName,
                methodParameters,
                controllerAreaName);
        }

        public static MethodCacheEntry Invalid() => InvalidInstance;
    }
}
