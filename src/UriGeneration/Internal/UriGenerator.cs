using UriGeneration.Internal.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;

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
           HttpContext httpContext,
           Expression<Action<TController>> action,
           string? endpointName = null,
           PathString? pathBase = null,
           FragmentString fragment = default,
           UriOptions? options = null)
               where TController : ControllerBase
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetPathByExpressionCore<TController>(
                httpContext,
                action,
                endpointName,
                pathBase,
                fragment,
                options);
        }

        public string? GetPathByExpression<TController>(
            HttpContext httpContext,
            Expression<Func<TController, object?>> action,
            string? endpointName = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : ControllerBase
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetPathByExpressionCore<TController>(
                httpContext,
                action,
                endpointName,
                pathBase,
                fragment,
                options);
        }

        public string? GetUriByExpression<TController>(
            HttpContext httpContext,
            Expression<Action<TController>> action,
            string? endpointName = null,
            string? scheme = null,
            HostString? host = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : ControllerBase
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetUriByExpressionCore<TController>(
                httpContext,
                action,
                endpointName,
                scheme,
                host,
                pathBase,
                fragment,
                options);
        }

        public string? GetUriByExpression<TController>(
            HttpContext httpContext,
            Expression<Func<TController, object?>> action,
            string? endpointName = null,
            string? scheme = null,
            HostString? host = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : ControllerBase
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetUriByExpressionCore<TController>(
                httpContext,
                action,
                endpointName,
                scheme,
                host,
                pathBase,
                fragment,
                options);
        }

        private string? GetPathByExpressionCore<TController>(
            HttpContext httpContext,
            LambdaExpression action,
            string? endpointName = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : ControllerBase
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (!_valuesExtractor.TryExtractValues<TController>(
                action,
                out var values,
                endpointName,
                options))
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
                    options: options);
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
                    options: options);
            }
        }

        private string? GetUriByExpressionCore<TController>(
            HttpContext httpContext,
            LambdaExpression action,
            string? endpointName = null,
            string? scheme = null,
            HostString? host = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : ControllerBase
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            if (!_valuesExtractor.TryExtractValues<TController>(
                action,
                out var values,
                endpointName,
                options))
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
                    options: options);
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
                     options: options);
            }
        }
    }
}
