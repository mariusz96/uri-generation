using Microsoft.Extensions.Caching.Memory;

namespace UriGeneration.Internal.Abstractions
{
    internal interface IMethodCacheAccessor : IDisposable
    {
        IMemoryCache Cache { get; }
    }
}
