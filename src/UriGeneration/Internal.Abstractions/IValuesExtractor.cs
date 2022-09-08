using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;

namespace UriGeneration.Internal.Abstractions
{
    internal interface IValuesExtractor
    {
        bool TryExtractValues<TController>(LambdaExpression action, [NotNullWhen(true)] out Values? values, string? endpointName = null, UriOptions? options = null) where TController : ControllerBase;
    }
}
