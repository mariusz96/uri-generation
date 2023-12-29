using Microsoft.AspNetCore.Routing;

namespace UriGeneration
{
    public class UriOptions
    {
        public bool? BypassMethodCache { get; set; }
        public bool? BypassCachedExpressionCompiler { get; set; }
        public LinkOptions? LinkOptions { get; set; }
    }
}
