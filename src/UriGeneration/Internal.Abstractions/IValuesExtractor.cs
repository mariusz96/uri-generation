using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace UriGeneration.Internal.Abstractions
{
    internal interface IValuesExtractor
    {
        bool TryExtractValues<TController>(LambdaExpression action, [NotNullWhen(true)] out Values? values, UriOptions? options = null) where TController : class;
    }
}
