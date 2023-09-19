using Microsoft.AspNetCore.Mvc;

namespace UriGeneration.IntegrationTests.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AnotherAttributeRoutingController : ControllerBase
    {
        [HttpGet]
        public string? Action() => null;
    }
}
