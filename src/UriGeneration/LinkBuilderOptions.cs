using Microsoft.AspNetCore.Routing;

namespace UriGeneration
{
    public class LinkBuilderOptions : LinkOptions
    {
        public static LinkBuilderOptions Default { get; } =
            new()
            {
                BypassCachedExpressionCompiler = false,
                BypassMethodCache = false
            };

        public bool BypassCachedExpressionCompiler { get; set; }
        public bool BypassMethodCache { get; set; }
    }
}
