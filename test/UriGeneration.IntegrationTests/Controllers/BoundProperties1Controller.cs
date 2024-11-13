using Microsoft.AspNetCore.Mvc;
using UriGeneration.IntegrationTests.Models;

namespace UriGeneration.IntegrationTests.Controllers
{
    public class BoundProperties1Controller : Controller
    {
        private readonly IUriGenerator _uriGenerator;

        public BoundProperties1Controller(IUriGenerator uriGenerator)
        {
            _uriGenerator = uriGenerator;
        }

        public TestModel? Model { get; set; }

        public string? Test1()
        {
            return _uriGenerator.GetUriByExpression<BoundProperties1Controller>(
                HttpContext,
                controller => controller.Test1());
        }
    }
}
