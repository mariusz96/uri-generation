using UriGeneration.Internal;
using UriGeneration.Internal.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace UriGeneration
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Action<MethodCacheOptions>
            DefaultConfigureCache = o =>
            {
                o.SizeLimit = 100;
                o.CompactionPercentage = 0.75;
            };

        public static IServiceCollection AddUriGeneration(
            this IServiceCollection services,
            Action<MethodCacheOptions>? configureCache = null)
        {
            configureCache ??= DefaultConfigureCache;

            services.AddOptions();
            services.Configure(configureCache);
            services.TryAddSingleton<IMethodCacheAccessor, MethodCacheAccessor>();
            services.TryAddSingleton<IValuesExtractor, ValuesExtractor>();
            services.TryAddSingleton<IUriGenerator, UriGenerator>();

            return services;
        }
    }
}
