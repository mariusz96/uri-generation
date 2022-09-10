using System.Reflection;

namespace UriGeneration.Internal
{
    internal record MethodCacheKey
    {
        public MethodInfo Method { get; }
        public Type Controller { get; }
        public string? EndpointName { get; }

        public MethodCacheKey(
            MethodInfo method,
            Type controller,
            string? endpointName = null)
        {
            Method = method;
            Controller = controller;
            EndpointName = endpointName;
        }
    }
}
