using Microsoft.AspNetCore.Mvc;
using UriGeneration.IntegrationTests.Models;

namespace UriGeneration.IntegrationTests.Controllers
{
    public class BoundProperties2Controller : Controller
    {
        private readonly IUriGenerator _uriGenerator;

        public BoundProperties2Controller(IUriGenerator uriGenerator)
        {
            _uriGenerator = uriGenerator;
        }

        [BindProperty]
        public TestModel? Model { get; set; }

        public string? Test1()
        {
            return _uriGenerator.GetUriByExpression<BoundProperties2Controller>(
                HttpContext,
                controller => controller.Test1());
        }
    }
}
