using Microsoft.AspNetCore.Routing;

namespace UriGeneration
{
    public class UriOptions : LinkOptions
    {
        public static UriOptions Default { get; } = new()
        {
            BypassCachedExpressionCompiler = false,
            BypassMethodCache = false
        };

        public bool BypassCachedExpressionCompiler { get; set; }
        public bool BypassMethodCache { get; set; }
    }
}
