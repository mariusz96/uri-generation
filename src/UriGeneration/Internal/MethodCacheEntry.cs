using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace UriGeneration.Internal
{
    internal partial class MethodCacheEntry
    {
        [MemberNotNullWhen(
            true,
            nameof(MethodName),
            nameof(ControllerName),
            nameof(IncludedMethodParameters))]
        public bool IsValid { get; }
        public string? MethodName { get; }
        public string? ControllerName { get; }
        public ParameterInfo[]? IncludedMethodParameters { get; }
        public string? ControllerAreaName { get; }

        private MethodCacheEntry(bool isValid)
        {
            IsValid = isValid;
        }

        private MethodCacheEntry(
            bool isValid,
            string methodName,
            string controllerName,
            ParameterInfo[] includedMethodParameters,
            string? controllerAreaName = null)
        {
            IsValid = isValid;
            MethodName = methodName;
            ControllerName = controllerName;
            IncludedMethodParameters = includedMethodParameters;
            ControllerAreaName = controllerAreaName;
        }
    }
}
