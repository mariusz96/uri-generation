using UriGeneration.Internal.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace UriGeneration.Internal
{
    internal sealed class MethodCacheAccessor : IMethodCacheAccessor, IDisposable
    {
        public IMemoryCache Cache { get; }

        public MethodCacheAccessor(IOptions<UriGenerationOptions> globalOptionsAccessor)
        {
            if (globalOptionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(globalOptionsAccessor));
            }

            var globalOptions = globalOptionsAccessor.Value;

            Cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = globalOptions.MethodCacheSizeLimit ?? 500
            });
        }

        public void Dispose() => Cache?.Dispose();
    }
}
