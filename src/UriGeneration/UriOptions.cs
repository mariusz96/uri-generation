using Microsoft.AspNetCore.Routing;

namespace UriGeneration
{
    /// <summary>
    /// Options for <see cref="IUriGenerator"/>.
    /// </summary>
    public class UriOptions
    {
        /// <summary>
        /// Gets or sets a value indicating whether MethodCache should be bypassed.
        /// </summary>
        /// <value>Defaults to <see langword="null"/>.</value>
        public bool? BypassMethodCache { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether CachedExpressionCompiler should be bypassed.
        /// </summary>
        /// <value>Defaults to <see langword="null"/>.</value>
        public bool? BypassCachedExpressionCompiler { get; set; }

        /// <summary>
        /// Gets or sets <see cref="Microsoft.AspNetCore.Routing.LinkOptions"/>.
        /// </summary>
        /// <value>Defaults to <see langword="null"/>.</value>
        public LinkOptions? LinkOptions { get; set; }
    }
}
