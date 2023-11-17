using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UriGeneration.IntegrationTests.Models;

namespace UriGeneration.IntegrationTests.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class AttributeRoutingController : ControllerBase
    {
        private readonly IUriGenerator _uriGenerator;

        public AttributeRoutingController(IUriGenerator uriGenerator)
        {
            _uriGenerator = uriGenerator;
        }

        [HttpGet]
        public string? Test1()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test1());
        }

        [HttpGet]
        public string? Test2()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.VoidAction());
        }

        [HttpPost]
        public void VoidAction()
        {
        }

        [HttpGet]
        public string? Test3()
        {
            return _uriGenerator
                .GetUriByExpression<AnotherAttributeRoutingController>(
                    HttpContext,
                    c => c.Action());
        }

        [HttpGet]
        public string? Test4()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                c => c.Test4(),
                endpointName: null,
                "http",
                new HostString("localhost"));
        }

        [HttpGet]
        public string? Test5(int id)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test5(id));
        }

        [HttpGet]
        public string? Test6([FromQuery] int id)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test6(id));
        }

        [HttpGet("{id}")]
        public string? Test7(int id)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test7(id));
        }

        [HttpGet("{id}")]
        public string? Test8([FromRoute] int id)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test8(id));
        }

        [ActionName("ActionName")]
        [HttpGet]
        public string? Test9()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test9());
        }

        [HttpPost]
        public string? Test10([FromBody] TestModel model)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test10(model));
        }

        [HttpPost]
        public string? Test11([FromForm] TestModel model)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test11(model));
        }

        [HttpGet]
        public string? Test12([FromHeader] string apiKey)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test12(apiKey));
        }

        [HttpGet]
        public string? Test13([FromServices] IUriGenerator uriGenerator)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test13(uriGenerator));
        }

        [HttpPost]
        public string? Test14(IFormFile file)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test14(file));
        }

        [HttpPost]
        public string? Test15(IFormFileCollection files)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test15(files));
        }

        [HttpPost]
        public string? Test16(IEnumerable<IFormFile> files)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test16(files));
        }

        [HttpGet]
        public string? Test17(CancellationToken cancellationToken)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test17(cancellationToken));
        }

        [HttpPost]
        public string? Test18(IFormCollection form)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test18(form));
        }

        [HttpGet("/EndpointName1/[action]", Name = "En1")]
        [HttpGet("/EndpointName2/[action]", Name = "En2")]
        public string? Test19(string endpointName)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test19(endpointName),
                endpointName);
        }

        [HttpGet]
        public Task<string?> Test20Async()
        {
            return Task.FromResult(
                _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                    HttpContext,
                    c => c.Test20Async()));
        }

        [HttpGet]
        public string? Test21()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test21(),
                options: new UriOptions
                {
                    AppendTrailingSlash = true
                });
        }

        [HttpGet]
        public string? Test22(int id)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test22(id),
                options: new UriOptions
                {
                    BypassMethodCache = true,
                    BypassCachedExpressionCompiler = true
                });
        }

        [TestTemplateProvider("/TemplateProvider/[action]")]
        public string? Test23()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test23());
        }
    }
}
