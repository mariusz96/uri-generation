using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace UriGeneration
{
    /// <summary>
    /// Defines a contract to generate absolute and related URIs based on endpoint routing and lambda expressions.
    /// </summary>
    public interface IUriGenerator
    {
        /// <summary>
        /// Generates a URI with an absolute path based on the provided values.
        /// </summary>
        /// <typeparam name="TController">The controller type.</typeparam> 
        /// <param name="expression">The lambda expression. Used to resolve endpoints and expand parameters in the route template.</param>
        /// <param name="endpointName">The endpoint name. Optional. Used to resolve endpoints.</param>
        /// <param name="pathBase">An optional URI path base. Prepended to the path in the resulting URI.</param>
        /// <param name="fragment">An optional URI fragment. Appended to the resulting URI.</param>
        /// <param name="options">An optional <see cref="UriOptions"/>. Settings on provided object override the settings with matching names from <see cref="UriGenerationOptions"/>.</param>
        /// <returns>A URI with an absolute path, or <c>null</c> if a URI cannot be created.</returns>
        string? GetPathByExpression<TController>(Expression<Action<TController>> expression, string? endpointName = null, PathString pathBase = default, FragmentString fragment = default, UriOptions? options = null) where TController : class;

        /// <summary>
        /// Generates a URI with an absolute path based on the provided values and <see cref="HttpContext"/>.
        /// </summary>
        /// <typeparam name="TController">The controller type.</typeparam>
        /// <param name="httpContext">The <see cref="HttpContext"/> associated with the current request.</param>        
        /// <param name="expression">The lambda expression. Used to resolve endpoints and expand parameters in the route template.</param>        
        /// <param name="endpointName">The endpoint name. Optional. Used to resolve endpoints.</param>
        /// <param name="pathBase">An optional URI path base. Prepended to the path in the resulting URI. If not provided, the value of <see cref="HttpRequest.PathBase"/> will be used.</param>
        /// <param name="fragment">An optional URI fragment. Appended to the resulting URI.</param>
        /// <param name="options">An optional <see cref="UriOptions"/>. Settings on provided object override the settings with matching names from <see cref="UriGenerationOptions"/>.</param>
        /// <returns>A URI with an absolute path, or <c>null</c> if a URI cannot be created.</returns>
        string? GetPathByExpression<TController>(HttpContext httpContext, Expression<Action<TController>> expression, string? endpointName = null, PathString? pathBase = null, FragmentString fragment = default, UriOptions? options = null) where TController : class;

        /// <summary>
        /// Generates an absolute URI based on the provided values.
        /// </summary>
        /// <remarks>The value of <paramref name="host" /> should be a trusted value. Relying on the value of the current request can allow untrusted input to influence the resulting URI unless the <c>Host</c> header has been validated. See the deployment documentation for instructions on how to properly validate the <c>Host</c> header in your deployment environment.</remarks>
        /// <typeparam name="TController">The controller type.</typeparam>
        /// <param name="expression">The lambda expression. Used to resolve endpoints and expand parameters in the route template.</param>        
        /// <param name="endpointName">The endpoint name. Optional. Used to resolve endpoints.</param>        
        /// <param name="scheme">The URI scheme, applied to the resulting URI.</param>        
        /// <param name="host">The URI host/authority, applied to the resulting URI. See the remarks section for details about the security implications of the <paramref name="host"/>.</param>
        /// <param name="pathBase">An optional URI path base. Prepended to the path in the resulting URI.</param>
        /// <param name="fragment">An optional URI fragment. Appended to the resulting URI.</param>
        /// <param name="options">An optional <see cref="UriOptions"/>. Settings on provided object override the settings with matching names from <see cref="UriGenerationOptions"/>.</param>
        /// <returns>A absolute URI, or <c>null</c> if a URI cannot be created.</returns>
        string? GetUriByExpression<TController>(Expression<Action<TController>> expression, string? endpointName, string scheme, HostString host, PathString pathBase = default, FragmentString fragment = default, UriOptions? options = null) where TController : class;

        /// <summary>
        /// Generates an absolute URI based on the provided values and <see cref="HttpContext"/>.
        /// </summary>
        /// <remarks>The value of <paramref name="host" /> should be a trusted value. Relying on the value of the current request can allow untrusted input to influence the resulting URI unless the <c>Host</c> header has been validated. See the deployment documentation for instructions on how to properly validate the <c>Host</c> header in your deployment environment.</remarks>
        /// <typeparam name="TController">The controller type.</typeparam>
        /// <param name="httpContext">The <see cref="HttpContext"/> associated with the current request.</param>        
        /// <param name="expression">The lambda expression. Used to resolve endpoints and expand parameters in the route template.</param>         
        /// <param name="endpointName">The endpoint name. Optional. Used to resolve endpoints.</param>        
        /// <param name="scheme">The URI scheme, applied to the resulting URI. Optional. If not provided, the value of <see cref="HttpRequest.Scheme"/> will be used.</param>
        /// <param name="host">The URI host/authority, applied to the resulting URI. Optional. If not provided, the value <see cref="HttpRequest.Host"/> will be used. See the remarks section for details about the security implications of the <paramref name="host"/>.</param>
        /// <param name="pathBase">An optional URI path base. Prepended to the path in the resulting URI. If not provided, the value of <see cref="HttpRequest.PathBase"/> will be used.</param>
        /// <param name="fragment">An optional URI fragment. Appended to the resulting URI.</param>
        /// <param name="options">An optional <see cref="UriOptions"/>. Settings on provided object override the settings with matching names from <see cref="UriGenerationOptions"/>.</param>
        /// <returns>A absolute URI, or <c>null</c> if a URI cannot be created.</returns>
        string? GetUriByExpression<TController>(HttpContext httpContext, Expression<Action<TController>> expression, string? endpointName = null, string? scheme = null, HostString? host = null, PathString? pathBase = null, FragmentString fragment = default, UriOptions? options = null) where TController : class;
    }
}
