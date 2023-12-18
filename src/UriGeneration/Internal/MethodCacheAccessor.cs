using UriGeneration.Internal.Abstractions;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace UriGeneration.Internal
{
    internal sealed class MethodCacheAccessor : IMethodCacheAccessor, IDisposable
    {
        public IMemoryCache Cache { get; }

        public MethodCacheAccessor(IOptions<UriGenerationOptions> globalOptions)
        {
            if (globalOptions == null)
            {
                throw new ArgumentNullException(nameof(globalOptions));
            }

            var options = globalOptions.Value;

            Cache = new MemoryCache(new MemoryCacheOptions
            {
                SizeLimit = options.MethodCacheSizeLimit ?? 500,
                CompactionPercentage = options.MethodCacheCompactionPercentage ?? 0.5
            });
        }

        public void Dispose() => Cache?.Dispose();
    }
}
