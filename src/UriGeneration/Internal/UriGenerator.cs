using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;
using UriGeneration.Internal.Abstractions;

namespace UriGeneration.Internal
{
    internal class UriGenerator : IUriGenerator
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IValuesExtractor _valuesExtractor;

        public UriGenerator(
            LinkGenerator linkGenerator,
            IValuesExtractor valuesExtractor)
        {
            if (linkGenerator == null)
            {
                throw new ArgumentNullException(nameof(linkGenerator));
            }

            if (valuesExtractor == null)
            {
                throw new ArgumentNullException(nameof(valuesExtractor));
            }

            _linkGenerator = linkGenerator;
            _valuesExtractor = valuesExtractor;
        }

        public string? GetPathByExpression<TController>(
            Expression<Action<TController>> expression,
            string? endpointName = null,
            PathString pathBase = default,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!_valuesExtractor.TryExtractValues(
                httpContext: null,
                expression,
                options,
                out var values))
            {
                return default;
            }

            if (endpointName != null)
            {
                return _linkGenerator.GetPathByName(
                    endpointName: endpointName,
                    values: values.RouteValues,
                    pathBase: pathBase,
                    fragment: fragment,
                    options: options?.LinkOptions);
            }
            else
            {
                return _linkGenerator.GetPathByAction(
                    action: values.ActionName,
                    controller: values.ControllerName,
                    values: values.RouteValues,
                    pathBase: pathBase,
                    fragment: fragment,
                    options: options?.LinkOptions);
            }
        }

        public string? GetPathByExpression<TController>(
           HttpContext httpContext,
           Expression<Action<TController>> expression,
           string? endpointName = null,
           PathString? pathBase = null,
           FragmentString fragment = default,
           UriOptions? options = null)
               where TController : class
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!_valuesExtractor.TryExtractValues(
                httpContext,
                expression,
                options,
                out var values))
            {
                return default;
            }

            if (endpointName != null)
            {
                return _linkGenerator.GetPathByName(
                    httpContext: httpContext,
                    endpointName: endpointName,
                    values: values.RouteValues,
                    pathBase: pathBase,
                    fragment: fragment,
                    options: options?.LinkOptions);
            }
            else
            {
                return _linkGenerator.GetPathByAction(
                    httpContext: httpContext,
                    action: values.ActionName,
                    controller: values.ControllerName,
                    values: values.RouteValues,
                    pathBase: pathBase,
                    fragment: fragment,
                    options: options?.LinkOptions);
            }
        }

        public string? GetUriByExpression<TController>(
            Expression<Action<TController>> expression,
            string? endpointName,
            string scheme,
            HostString host,
            PathString pathBase = default,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : class
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!_valuesExtractor.TryExtractValues(
                httpContext: null,
                expression,
                options,
                out var values))
            {
                return default;
            }

            if (endpointName != null)
            {
                return _linkGenerator.GetUriByName(
                    endpointName: endpointName,
                    values: values.RouteValues,
                    scheme: scheme,
                    host: host,
                    pathBase: pathBase,
                    fragment: fragment,
                    options: options?.LinkOptions);
            }
            else
            {
                return _linkGenerator.GetUriByAction(
                     action: values.ActionName,
                     controller: values.ControllerName,
                     values: values.RouteValues,
                     scheme: scheme,
                     host: host,
                     pathBase: pathBase,
                     fragment: fragment,
                     options: options?.LinkOptions);
            }
        }

        public string? GetUriByExpression<TController>(
            HttpContext httpContext,
            Expression<Action<TController>> expression,
            string? endpointName = null,
            string? scheme = null,
            HostString? host = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : class
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!_valuesExtractor.TryExtractValues(
                httpContext,
                expression,
                options,
                out var values))
            {
                return default;
            }

            if (endpointName != null)
            {
                return _linkGenerator.GetUriByName(
                    httpContext: httpContext,
                    endpointName: endpointName,
                    values: values.RouteValues,
                    scheme: scheme,
                    host: host,
                    pathBase: pathBase,
                    fragment: fragment,
                    options: options?.LinkOptions);
            }
            else
            {
                return _linkGenerator.GetUriByAction(
                     httpContext: httpContext,
                     action: values.ActionName,
                     controller: values.ControllerName,
                     values: values.RouteValues,
                     scheme: scheme,
                     host: host,
                     pathBase: pathBase,
                     fragment: fragment,
                     options: options?.LinkOptions);
            }
        }
    }
}
