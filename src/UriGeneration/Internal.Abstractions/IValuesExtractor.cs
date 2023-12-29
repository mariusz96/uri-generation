using Microsoft.AspNetCore.Http;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace UriGeneration.Internal.Abstractions
{
    internal interface IValuesExtractor
    {
        bool TryExtractValues<TController>(HttpContext? httpContext, Expression<Action<TController>> expression, UriOptions? options, [NotNullWhen(true)] out Values? values) where TController : class;
    }
}
