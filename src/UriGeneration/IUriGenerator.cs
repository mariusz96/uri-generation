using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace UriGeneration
{
    public interface IUriGenerator
    {
        string? GetPathByExpression<TController>(Expression<Action<TController>> action, string? endpointName = null, PathString pathBase = default, FragmentString fragment = default, UriOptions? options = null) where TController : ControllerBase;
        string? GetPathByExpression<TController>(Expression<Func<TController, object?>> action, string? endpointName = null, PathString pathBase = default, FragmentString fragment = default, UriOptions? options = null) where TController : ControllerBase;
        string? GetPathByExpression<TController>(HttpContext httpContext, Expression<Action<TController>> action, string? endpointName = null, PathString? pathBase = null, FragmentString fragment = default, UriOptions? options = null) where TController : ControllerBase;
        string? GetPathByExpression<TController>(HttpContext httpContext, Expression<Func<TController, object?>> action, string? endpointName = null, PathString? pathBase = null, FragmentString fragment = default, UriOptions? options = null) where TController : ControllerBase;
        string? GetUriByExpression<TController>(Expression<Action<TController>> action, string? endpointName, string scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, UriOptions? options = null) where TController : ControllerBase;
        string? GetUriByExpression<TController>(Expression<Func<TController, object?>> action, string? endpointName, string scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, UriOptions? options = null) where TController : ControllerBase;
        string? GetUriByExpression<TController>(HttpContext httpContext, Expression<Action<TController>> action, string? endpointName = null, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, UriOptions? options = null) where TController : ControllerBase;
        string? GetUriByExpression<TController>(HttpContext httpContext, Expression<Func<TController, object?>> action, string? endpointName = null, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, UriOptions? options = null) where TController : ControllerBase;
    }
}
