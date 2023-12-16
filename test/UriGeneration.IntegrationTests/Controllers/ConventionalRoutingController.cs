using Microsoft.AspNetCore.Mvc;

namespace UriGeneration.IntegrationTests.Controllers
{
    public class ConventionalRoutingController : Controller
    {
        private readonly IUriGenerator _uriGenerator;

        public ConventionalRoutingController(IUriGenerator uriGenerator)
        {
            _uriGenerator = uriGenerator;
        }

        public string? Index()
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    c => c.Index());
        }

        public string? Test1()
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    c => c.Test1());
        }

        public string? Test2()
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    c => c.VoidAction());
        }

        public void VoidAction()
        {
        }

        public string? Test3()
        {
            return _uriGenerator
                .GetPathByExpression<AnotherConventionalRoutingController>(
                    c => c.Index());
        }

        public string? Test4()
        {
            return _uriGenerator
                .GetPathByExpression<AnotherConventionalRoutingController>(
                    HttpContext,
                    c => c.Action());
        }

        public string? Test5()
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    c => c.Test5());
        }

        public string? Test6(int value)
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    c => c.Test6(value));
        }

        public string? Test7(int[] values)
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    c => c.Test7(values));
        }

        public string? Test8(string routeName)
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    c => c.Test8(routeName),
                    routeName);
        }
    }
}
