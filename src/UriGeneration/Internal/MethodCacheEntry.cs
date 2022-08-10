using System.Reflection;

namespace UriGeneration.Internal
{
    internal class MethodCacheEntry
    {
        public string MethodName { get; set; }
        public string ControllerName { get; set; }
        public ParameterInfo[] MethodParameters { get; set; }
        public string? ControllerAreaName { get; set; }

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
