using System.Reflection;

namespace UriGeneration.Internal
{
    internal class MethodCacheEntry
    {
        public string MethodName { get; }
        public string ControllerName { get; }
        public ParameterInfo[] MethodParameters { get; }
        public string? ControllerAreaName { get; }

        public MethodCacheEntry(
            string methodName,
            string controllerName,
            ParameterInfo[] methodParameters,
            string? controllerAreaName = null)
        {
            MethodName = methodName;
            ControllerName = controllerName;
            MethodParameters = methodParameters;
            ControllerAreaName = controllerAreaName;
        }
    }
}
