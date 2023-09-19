using Microsoft.AspNetCore.Mvc;

namespace UriGeneration.IntegrationTests.Areas
{
    public class AreasController : Controller
    {
        private readonly IUriGenerator _uriGenerator;

        public AreasController(IUriGenerator uriGenerator)
        {
            _uriGenerator = uriGenerator;
        }

        public string? Action() => null;

        public string? Test1()
        {
            return _uriGenerator.GetUriByExpression<AreasController>(
                HttpContext,
                c => c.Action());
        }

        public string? Test2()
        {
            return _uriGenerator
                .GetUriByExpression<Area1.Controllers.AreasController>(
                    HttpContext,
                    c => c.Action());
        }
    }
}
