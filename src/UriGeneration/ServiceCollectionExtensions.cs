using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using UriGeneration.Internal;
using UriGeneration.Internal.Abstractions;

namespace UriGeneration
{
    /// <summary>
    /// Extension methods to <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds UriGeneration services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
        /// <param name="configure">The <see cref="UriGenerationOptions"/> to configure the services with.</param>
        /// <returns>The <see cref="IServiceCollection"/> so that additional calls can be chained.</returns>
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
