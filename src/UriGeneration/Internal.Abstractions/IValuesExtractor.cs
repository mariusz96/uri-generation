using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace UriGeneration.Internal.Abstractions
{
    internal interface IValuesExtractor
    {
        Values ExtractValues<TController>(LambdaExpression action, string? endpointName = null, LinkBuilderOptions? options = null) where TController : ControllerBase;
    }
}
