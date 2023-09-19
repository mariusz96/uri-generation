using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using System.Reflection;
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

        [ActionName("ActionName")]
        [HttpGet]
        public string? Test8()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test8());
        }

        [HttpPost]
        public string? Test9([FromBody] TestModel model)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test9(model));
        }

        [HttpPost]
        public string? Test10([FromForm] TestModel model)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test10(model));
        }

        [HttpGet]
        public string? Test11([FromHeader] string apiKey)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test11(apiKey));
        }

        [HttpGet]
        public string? Test12([FromServices] IUriGenerator uriGenerator)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test12(uriGenerator));
        }

        [HttpPost]
        public string? Test13(IFormFile file)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test13(file));
        }

        [HttpPost]
        public string? Test14(IFormFileCollection files)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test14(files));
        }

        [HttpPost]
        public string? Test15(IEnumerable<IFormFile> files)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test15(files));
        }

        [HttpGet]
        public string? Test16(CancellationToken cancellationToken)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test16(cancellationToken));
        }

        [HttpPost]
        public string? Test17(IFormCollection form)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test17(form));
        }

        [HttpGet("/EndpointName1/[action]", Name = "En1")]
        [HttpGet("/EndpointName2/[action]", Name = "En2")]
        public string? Test18(string endpointName)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test18(endpointName),
                endpointName);
        }

        [HttpGet]
        public Task<string?> Test19Async()
        {
            return Task.FromResult(
                _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                    HttpContext,
                    c => c.Test19Async()));
        }

        [HttpGet]
        public string? Test20()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test20(),
                options: new UriOptions
                {
                    AppendTrailingSlash = true
                });
        }

        [HttpGet]
        public string? Test21(int id)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test21(id),
                options: new UriOptions
                {
                    BypassMethodCache = true,
                    BypassCachedExpressionCompiler = true
                });
        }

        [TestTemplateProvider("/TemplateProvider/[action]")]
        public string? Test22()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test22());
        }
    }
}
