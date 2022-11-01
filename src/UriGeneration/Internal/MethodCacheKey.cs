using System.Reflection;

namespace UriGeneration.Internal
{
    internal record MethodCacheKey
    {
        public MethodInfo Method { get; }
        public Type Controller { get; }

        public MethodCacheKey(MethodInfo method, Type controller)
        {
            Method = method;
            Controller = controller;
        }
    }
}
