using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace UriGeneration
{
    public interface ILinkBuilder
    {
        string BuildPathFromExpression<TController>(HttpContext httpContext, Expression<Action<TController>> action, string? endpointName = null, PathString? pathBase = null, FragmentString fragment = default, LinkBuilderOptions? options = null) where TController : ControllerBase;
        string BuildPathFromExpression<TController>(HttpContext httpContext, Expression<Func<TController, object?>> action, string? endpointName = null, PathString? pathBase = null, FragmentString fragment = default, LinkBuilderOptions? options = null) where TController : ControllerBase;
        string BuildUriFromExpression<TController>(HttpContext httpContext, Expression<Action<TController>> action, string? endpointName = null, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, LinkBuilderOptions? options = null) where TController : ControllerBase;
        string BuildUriFromExpression<TController>(HttpContext httpContext, Expression<Func<TController, object?>> action, string? endpointName = null, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, LinkBuilderOptions? options = null) where TController : ControllerBase;
    }
}
