using Microsoft.AspNetCore.Mvc;

namespace UriGeneration.IntegrationTests.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class SlugifyController : ControllerBase
    {
        private readonly IUriGenerator _uriGenerator;

        public SlugifyController(IUriGenerator uriGenerator)
        {
            _uriGenerator = uriGenerator;
        }

        [HttpGet]
        public string? SlugTest1()
        {
            return _uriGenerator.GetUriByExpression<SlugifyController>(
                HttpContext,
                controller => controller.SlugTest1());
        }
    }
}
