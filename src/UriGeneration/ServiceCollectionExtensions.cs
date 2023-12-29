using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UriGeneration.Internal;
using UriGeneration.Internal.Abstractions;

namespace UriGeneration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUriGeneration(
            this IServiceCollection services,
            Action<UriGenerationOptions>? configure = null)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddOptions();

            if (configure != null)
            {
                services.Configure(configure);
            }

            services.TryAddSingleton<IMethodCacheAccessor, MethodCacheAccessor>();
            services.TryAddSingleton<IValuesExtractor, ValuesExtractor>();
            services.TryAddSingleton<IUriGenerator, UriGenerator>();

            return services;
        }
    }
}
