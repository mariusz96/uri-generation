using UriGeneration.Internal;
using UriGeneration.Internal.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace UriGeneration
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Action<MethodCacheOptions> DefaultConfigure =
            o =>
            {
                o.SizeLimit = 500;
                o.CompactionPercentage = 0.75;
            };

        public static IServiceCollection AddUriGeneration(
            this IServiceCollection services,
            Action<MethodCacheOptions>? configure = null)
        {
            configure ??= DefaultConfigure;

            services.AddOptions();
            services.Configure(configure);
            services.TryAddSingleton<IMethodCacheAccessor, MethodCacheAccessor>();
            services.TryAddSingleton<IValuesExtractor, ValuesExtractor>();
            services.TryAddSingleton<IUriGenerator, UriGenerator>();

            return services;
        }
    }
}
