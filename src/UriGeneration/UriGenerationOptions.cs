namespace UriGeneration
{
    public class UriGenerationOptions
    {
        public long? MethodCacheSizeLimit { get; set; }
        public double? MethodCacheCompactionPercentage { get; set; }
        public bool? BypassMethodCache { get; set; }
        public bool? BypassCachedExpressionCompiler { get; set; }
    }
}
