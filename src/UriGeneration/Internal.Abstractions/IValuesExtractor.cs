using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace UriGeneration.Internal.Abstractions
{
    internal interface IValuesExtractor
    {
        bool TryExtractValues<TController>(LambdaExpression action, out Values values, string? endpointName = null, UriOptions? options = null) where TController : ControllerBase;
    }
}
