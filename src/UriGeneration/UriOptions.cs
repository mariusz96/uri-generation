using Microsoft.AspNetCore.Routing;

namespace UriGeneration
{
    public class UriOptions : LinkOptions
    {
        public bool? BypassMethodCache { get; set; }
        public bool? BypassCachedExpressionCompiler { get; set; }
    }
}
