using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UriGeneration.IntegrationTests.Models;

#if NET8_0_OR_GREATER
using Microsoft.Extensions.DependencyInjection;
using UriGeneration.IntegrationTests.Services;
#endif

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
                controller => controller.Test1());
        }

        [HttpGet]
        public string? Test2()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.VoidAction());
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
                    controller => controller.Action());
        }

        [HttpGet]
        public string? Test4()
        {
            string scheme = "http";
            var host = new HostString("localhost");

            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                controller => controller.Test4(),
                endpointName: null,
                scheme,
                host);
        }

        [HttpGet]
        public string? Test5(int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test5(value));
        }

        [HttpGet]
        public string? Test6([FromQuery] int[] values)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test6(values));
        }

        [HttpGet]
        public string? Test7([FromQuery] int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test7(value));
        }

        [HttpGet]
        public string? Test8([FromQuery(Name = "value2")] int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test8(value));
        }

        [HttpGet("{value}")]
        public string? Test9(int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test9(value));
        }

        [HttpGet("{value}")]
        public string? Test10([FromRoute] int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test10(value));
        }

        [HttpGet("{value2}")]
        public string? Test11([FromRoute(Name = "value2")] int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test11(value));
        }

        [ActionName("ActionName")]
        [HttpGet]
        public string? Test12()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test12());
        }

        [HttpPost]
        public string? Test13([FromBody] TestModel model)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test13(model));
        }

        [HttpPost]
        public string? Test14([FromForm] TestModel model)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test14(model));
        }

        [HttpGet]
        public string? Test15([FromHeader] string value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test15(value));
        }

        [HttpGet]
        public string? Test16([FromServices] IUriGenerator service)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test16(service));
        }

#if NET8_0_OR_GREATER
        [HttpGet]
        public string? Test17([FromKeyedServices("test")] ITestService service)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test17(service));
        }
#endif

        [HttpPost]
        public string? Test18(IFormFile file)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test18(file));
        }

        [HttpPost]
        public string? Test19(IFormFileCollection files)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test19(files));
        }

        [HttpPost]
        public string? Test20(IEnumerable<IFormFile> files)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test20(files));
        }

        [HttpGet]
        public string? Test21(CancellationToken cancellationToken)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test21(cancellationToken));
        }

        [HttpPost]
        public string? Test22(IFormCollection form)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test22(form));
        }

        [HttpGet("/EndpointName1/[action]", Name = "En1")]
        [HttpGet("/EndpointName2/[action]", Name = "En2")]
        public string? Test23(string endpointName)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test23(endpointName),
                endpointName);
        }

        [HttpGet]
        public Task<string?> Test24Async()
        {
            return Task.FromResult(
                _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                    HttpContext,
                    controller => controller.Test24Async()));
        }

        [HttpGet]
        public string? Test25()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test25(),
                options: new UriOptions
                {
                    LinkOptions = new LinkOptions
                    {
                        AppendTrailingSlash = true
                    }
                });
        }

        [HttpGet]
        public string? Test26(int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test26(value),
                options: new UriOptions
                {
                    BypassMethodCache = true,
                    BypassCachedExpressionCompiler = true
                });
        }

        [HttpGet]
        [TestTemplateProvider("/TemplateProvider/[action]")]
        public string? Test27()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test27());
        }

        [HttpGet]
        public string? Test28([TestBindingSource("Header")] string value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test28(value));
        }

        [HttpGet]
        public string? Test29([TestModelNameProvider("value2")] int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                controller => controller.Test29(value));
        }
    }
}
