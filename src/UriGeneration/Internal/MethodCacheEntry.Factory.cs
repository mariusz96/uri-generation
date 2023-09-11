using System.Reflection;

namespace UriGeneration.Internal
{
    internal partial class MethodCacheEntry
    {
        private static readonly MethodCacheEntry InvalidInstance =
            new(isValid: false);

        public static MethodCacheEntry Valid(
            string methodName,
            string controllerName,
            ParameterInfo[] includedMethodParameters,
            string controllerAreaName)
        {
            return new(
                isValid: true,
                methodName,
                controllerName,
                includedMethodParameters,
                controllerAreaName);
        }

        public static MethodCacheEntry Invalid() => InvalidInstance;
    }
}
