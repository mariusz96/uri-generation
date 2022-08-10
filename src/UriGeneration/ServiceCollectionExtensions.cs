using UriGeneration.Internal;
using UriGeneration.Internal.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace UriGeneration
{
    public static class ServiceCollectionExtensions
    {
        private static readonly Action<MethodCacheOptions>
            DefaultConfigureOptions = o =>
            {
                o.SizeLimit = 100;
                o.CompactionPercentage = 0.75;
            };

        public static IServiceCollection AddLinkBuilding(
            this IServiceCollection services,
            Action<MethodCacheOptions>? configureOptions = null)
        {
            configureOptions ??= DefaultConfigureOptions;

            services.AddOptions();
            services.Configure(configureOptions);
            services
                .TryAddSingleton<IMethodCacheAccessor, MethodCacheAccessor>();
            services.TryAddSingleton<IValuesExtractor, ValuesExtractor>();
            services.TryAddSingleton<ILinkBuilder, LinkBuilder>();

            return services;
        }
    }
}
