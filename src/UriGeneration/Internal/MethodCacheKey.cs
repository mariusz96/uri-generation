using System.Reflection;

namespace UriGeneration.Internal
{
    internal record MethodCacheKey(MethodInfo Method, Type Controller, int ActionDescriptorsVersion);
}
