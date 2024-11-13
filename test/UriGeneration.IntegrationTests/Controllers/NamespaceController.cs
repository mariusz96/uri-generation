using Microsoft.AspNetCore.Mvc;

namespace UriGeneration.IntegrationTests.Controllers
{
    public class NamespaceController : Controller
    {
        private readonly IUriGenerator _uriGenerator;

        public NamespaceController(IUriGenerator uriGenerator)
        {
            _uriGenerator = uriGenerator;
        }

        public string? Test1()
        {
            return _uriGenerator.GetUriByExpression<NamespaceController>(
                HttpContext,
                controller => controller.Test1());
        }
    }
}
