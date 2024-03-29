﻿using Microsoft.Extensions.Caching.Memory;

namespace UriGeneration.Internal.Abstractions
{
    internal interface IMethodCacheAccessor
    {
        IMemoryCache Cache { get; }
    }
}
