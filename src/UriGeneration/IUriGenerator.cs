using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace UriGeneration
{
    public interface IUriGenerator
    {
        string? GetPathByExpression<TController>(Expression<Action<TController>> expression, string? endpointName = null, PathString pathBase = default, FragmentString fragment = default, UriOptions? options = null) where TController : class;
        string? GetPathByExpression<TController>(HttpContext httpContext, Expression<Action<TController>> expression, string? endpointName = null, PathString? pathBase = null, FragmentString fragment = default, UriOptions? options = null) where TController : class;
        string? GetUriByExpression<TController>(Expression<Action<TController>> expression, string? endpointName, string scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, UriOptions? options = null) where TController : class;
        string? GetUriByExpression<TController>(HttpContext httpContext, Expression<Action<TController>> expression, string? endpointName = null, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, UriOptions? options = null) where TController : class;
    }
}
