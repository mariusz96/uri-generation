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
            Expression<Action<TController>> action,
            string? endpointName = null,
            PathString pathBase = default,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetPathByExpressionWithoutHttpContext<TController>(
                action,
                endpointName,
                pathBase,
                fragment,
                options);
        }

        public string? GetPathByExpression<TController>(
            Expression<Func<TController, object?>> action,
            string? endpointName = null,
            PathString pathBase = default,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetPathByExpressionWithoutHttpContext<TController>(
                action,
                endpointName,
                pathBase,
                fragment,
                options);
        }

        public string? GetPathByExpression<TController>(
           HttpContext httpContext,
           Expression<Action<TController>> action,
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

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetPathByExpressionWithHttpContext<TController>(
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
                where TController : class
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetPathByExpressionWithHttpContext<TController>(
                httpContext,
                action,
                endpointName,
                pathBase,
                fragment,
                options);
        }

        public string? GetUriByExpression<TController>(
            Expression<Action<TController>> action,
            string? endpointName,
            string scheme,
            HostString host,
            PathString pathBase = default,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetUriByExpressionWithoutHttpContext<TController>(
                action,
                endpointName,
                scheme,
                host,
                pathBase,
                fragment,
                options);
        }

        public string? GetUriByExpression<TController>(
            Expression<Func<TController, object?>> action,
            string? endpointName,
            string scheme,
            HostString host,
            PathString pathBase = default,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : class
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetUriByExpressionWithoutHttpContext<TController>(
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
            Expression<Action<TController>> action,
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

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetUriByExpressionWithHttpContext<TController>(
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
                where TController : class
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException(nameof(httpContext));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            return GetUriByExpressionWithHttpContext<TController>(
                httpContext,
                action,
                endpointName,
                scheme,
                host,
                pathBase,
                fragment,
                options);
        }

        private string? GetPathByExpressionWithoutHttpContext<TController>(
            LambdaExpression action,
            string? endpointName = null,
            PathString pathBase = default,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : class
        {
            if (!_valuesExtractor.TryExtractValues<TController>(
                action,
                out var values,
                options))
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

        private string? GetPathByExpressionWithHttpContext<TController>(
            HttpContext httpContext,
            LambdaExpression action,
            string? endpointName = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : class
        {
            if (!_valuesExtractor.TryExtractValues<TController>(
                action,
                out var values,
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

        private string? GetUriByExpressionWithoutHttpContext<TController>(
            LambdaExpression action,
            string? endpointName,
            string scheme,
            HostString host,
            PathString pathBase = default,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : class
        {
            if (!_valuesExtractor.TryExtractValues<TController>(
                action,
                out var values,
                options))
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

        private string? GetUriByExpressionWithHttpContext<TController>(
            HttpContext httpContext,
            LambdaExpression action,
            string? endpointName = null,
            string? scheme = null,
            HostString? host = null,
            PathString? pathBase = null,
            FragmentString fragment = default,
            UriOptions? options = null)
                where TController : class
        {
            if (!_valuesExtractor.TryExtractValues<TController>(
                action,
                out var values,
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
