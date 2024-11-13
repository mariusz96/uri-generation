using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UriGeneration
{
    /// <summary>
    /// Options for UriGeneration.
    /// </summary>
    public class UriGenerationOptions
    {
        /// <summary>
        /// Gets or sets the maximum size of MethodCache. Each MethodCache entry will have a size of 1.
        /// </summary>
        /// <value>Defaults to 500.</value>
        public long? MethodCacheSizeLimit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether MethodCache should be bypassed.
        /// </summary>
        /// <value>Defaults to <see langword="false"/>.</value>
        public bool? BypassMethodCache { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether CachedExpressionCompiler should be bypassed.
        /// </summary>
        /// <value>Defaults to <see langword="false"/>.</value>
        public bool? BypassCachedExpressionCompiler { get; set; }

        /// <summary>
        /// Gets or sets a predicate which can determine whether an action parameter should be included based on its binding source.
        /// </summary>
        /// <value>Defaults to a predicate which includes an action parameter if its binding source is <see langword="null"/> or can accept data from <see cref="BindingSource.Query"/> or <see cref="BindingSource.Path"/>.</value>
        public Func<BindingSource?, bool>? BindingSourceFilter { get; set; }
    }
}
