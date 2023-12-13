using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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

#if NET8_0_OR_GREATER
        [HttpGet]
        public string? Test14([Microsoft.Extensions.DependencyInjection.FromKeyedServices("test")] ITestService service)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test14(service));
        }
#endif

        [HttpPost]
        public string? Test15(IFormFile file)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test15(file));
        }

        [HttpPost]
        public string? Test16(IFormFileCollection files)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test16(files));
        }

        [HttpPost]
        public string? Test17(IEnumerable<IFormFile> files)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test17(files));
        }

        [HttpGet]
        public string? Test18(CancellationToken cancellationToken)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test18(cancellationToken));
        }

        [HttpPost]
        public string? Test19(IFormCollection form)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test19(form));
        }

        [HttpGet("/EndpointName1/[action]", Name = "En1")]
        [HttpGet("/EndpointName2/[action]", Name = "En2")]
        public string? Test20(string endpointName)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test20(endpointName),
                endpointName);
        }

        [HttpGet]
        public Task<string?> Test21Async()
        {
            return Task.FromResult(
                _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                    HttpContext,
                    c => c.Test21Async()));
        }

        [HttpGet]
        public string? Test22()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test22(),
                options: new UriOptions
                {
                    LinkOptions = new LinkOptions
                    {
                        AppendTrailingSlash = true
                    }
                });
        }

        [HttpGet]
        public string? Test23(int id)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test23(id),
                options: new UriOptions
                {
                    BypassMethodCache = true,
                    BypassCachedExpressionCompiler = true
                });
        }

        [HttpGet]
        [TestTemplateProvider("/TemplateProvider/[action]")]
        public string? Test24()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test24());
        }

        [HttpGet]
        public string? Test25([TestBindingSourceMetadata("Query")] int id)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test25(id));
        }
    }
}
