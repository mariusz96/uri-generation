using UriGeneration.Internal.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Linq.Expressions;

namespace UriGeneration.Internal
{
    internal class LinkBuilder : ILinkBuilder
    {
        private readonly LinkGenerator _linkGenerator;
        private readonly IValuesExtractor _valuesExtractor;

        public LinkBuilder(
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

        public string BuildPathFromExpression<TController>(
           HttpContext httpContext,
           Expression<Action<TController>> action,
           string? endpointName = null,
           PathString? pathBase = null,
           FragmentString fragment = default,
           LinkBuilderOptions? options = null)
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

            return BuildPathFromExpressionCore<TController>(
                httpContext,
                action,
                endpointName,
                pathBase,
                fragment,
                options);
        }

        public string BuildPathFromExpression<TController>(
            HttpContext httpContext,
            Expression<Func<TController, object?>> action,
            string? endpointName = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            LinkBuilderOptions? options = null)
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

            return BuildPathFromExpressionCore<TController>(
                httpContext,
                action,
                endpointName,
                pathBase,
                fragment,
                options);
        }

        public string BuildUriFromExpression<TController>(
            HttpContext httpContext,
            Expression<Action<TController>> action,
            string? endpointName = null,
            string? scheme = null,
            HostString? host = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            LinkBuilderOptions? options = null)
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

            return BuildUriFromExpressionCore<TController>(
                httpContext,
                action,
                endpointName,
                scheme,
                host,
                pathBase,
                fragment,
                options);
        }

        public string BuildUriFromExpression<TController>(
            HttpContext httpContext,
            Expression<Func<TController, object?>> action,
            string? endpointName = null,
            string? scheme = null,
            HostString? host = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            LinkBuilderOptions? options = null)
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

            return BuildUriFromExpressionCore<TController>(
                httpContext,
                action,
                endpointName,
                scheme,
                host,
                pathBase,
                fragment,
                options);
        }

        private string BuildPathFromExpressionCore<TController>(
            HttpContext httpContext,
            LambdaExpression action,
            string? endpointName = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            LinkBuilderOptions? options = null)
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

            var values = _valuesExtractor.ExtractValues<TController>(
                action,
                endpointName,
                options);

            string? path;

            if (endpointName != null)
            {
                path = _linkGenerator.GetPathByName(
                    httpContext: httpContext,
                    endpointName: endpointName,
                    values: values.RouteValues,
                    pathBase: pathBase,
                    fragment: fragment,
                    options: options);
            }
            else
            {
                path = _linkGenerator.GetPathByAction(
                    httpContext: httpContext,
                    action: values.ActionName,
                    controller: values.ControllerName,
                    values: values.RouteValues,
                    pathBase: pathBase,
                    fragment: fragment,
                    options: options);
            }

            if (path == null)
            {
                throw new InvalidOperationException("An URI could not be " +
                    "created.");
            }

            return path;
        }

        private string BuildUriFromExpressionCore<TController>(
            HttpContext httpContext,
            LambdaExpression action,
            string? endpointName = null,
            string? scheme = null,
            HostString? host = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            LinkBuilderOptions? options = null)
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

            var values = _valuesExtractor.ExtractValues<TController>(
                action,
                endpointName,
                options);

            string? uri;

            if (endpointName != null)
            {
                uri = _linkGenerator.GetUriByName(
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
                uri = _linkGenerator.GetUriByAction(
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

            if (uri == null)
            {
                throw new InvalidOperationException("An URI could not be " +
                    "created.");
            }

            return uri;
        }
    }
}
