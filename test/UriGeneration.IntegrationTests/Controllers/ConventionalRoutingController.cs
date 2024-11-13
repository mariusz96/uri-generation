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
                    controller => controller.Index());
        }

        public string? Test1()
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    controller => controller.Test1());
        }

        public string? Test2()
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    controller => controller.VoidAction());
        }

        public void VoidAction()
        {
        }

        public string? Test3()
        {
            return _uriGenerator
                .GetPathByExpression<AnotherConventionalRoutingController>(
                    HttpContext,
                    controller => controller.Index());
        }

        public string? Test4()
        {
            return _uriGenerator
                .GetPathByExpression<AnotherConventionalRoutingController>(
                    HttpContext,
                    controller => controller.Action());
        }

        public string? Test5()
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    controller => controller.Test5());
        }

        public string? Test6(int value)
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    controller => controller.Test6(value));
        }

        public string? Test7(int[] values)
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    controller => controller.Test7(values));
        }

        public string? Test8(string routeName)
        {
            return _uriGenerator
                .GetPathByExpression<ConventionalRoutingController>(
                    HttpContext,
                    controller => controller.Test8(routeName),
                    routeName);
        }
    }
}
