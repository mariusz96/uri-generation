using Microsoft.AspNetCore.Mvc;
using UriGeneration.IntegrationTests.Models;

namespace UriGeneration.IntegrationTests.Controllers
{
    [BindProperties]
    public class BoundProperties3Controller : Controller
    {
        private readonly IUriGenerator _uriGenerator;

        public BoundProperties3Controller(IUriGenerator uriGenerator)
        {
            _uriGenerator = uriGenerator;
        }

        public TestModel? Model { get; set; }

        public string? Test1()
        {
            return _uriGenerator.GetUriByExpression<BoundProperties3Controller>(
                HttpContext,
                controller => controller.Test1());
        }
    }
}
