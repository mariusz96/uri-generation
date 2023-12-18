﻿using Microsoft.AspNetCore.Http;
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
            string scheme = "http";
            var host = new HostString("localhost");

            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                c => c.Test4(),
                endpointName: null,
                scheme,
                host);
        }

        [HttpGet]
        public string? Test5(int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test5(value));
        }

        [HttpGet]
        public string? Test6([FromQuery] int[] values)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test6(values));
        }

        [HttpGet]
        public string? Test7([FromQuery] int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test7(value));
        }

        [HttpGet]
        public string? Test8([FromQuery(Name = "value2")] int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test8(value));
        }

        [HttpGet("{value}")]
        public string? Test9(int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test9(value));
        }

        [HttpGet("{value}")]
        public string? Test10([FromRoute] int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test10(value));
        }

        [HttpGet("{value2}")]
        public string? Test11([FromRoute(Name = "value2")] int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test11(value));
        }

        [ActionName("ActionName")]
        [HttpGet]
        public string? Test12()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test12());
        }

        [HttpPost]
        public string? Test13([FromBody] TestModel model)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test13(model));
        }

        [HttpPost]
        public string? Test14([FromForm] TestModel model)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test14(model));
        }

        [HttpGet]
        public string? Test15([FromHeader] string value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test15(value));
        }

        [HttpGet]
        public string? Test16([FromServices] IUriGenerator service)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test16(service));
        }

#if NET8_0_OR_GREATER
        [HttpGet]
        public string? Test17([FromKeyedServices("test")] ITestService service)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test17(service));
        }
#endif

        [HttpPost]
        public string? Test18(IFormFile file)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test18(file));
        }

        [HttpPost]
        public string? Test19(IFormFileCollection files)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test19(files));
        }

        [HttpPost]
        public string? Test20(IEnumerable<IFormFile> files)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test20(files));
        }

        [HttpGet]
        public string? Test21(CancellationToken cancellationToken)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test21(cancellationToken));
        }

        [HttpPost]
        public string? Test22(IFormCollection form)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test22(form));
        }

        [HttpGet("/EndpointName1/[action]", Name = "En1")]
        [HttpGet("/EndpointName2/[action]", Name = "En2")]
        public string? Test23(string endpointName)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test23(endpointName),
                endpointName);
        }

        [HttpGet]
        public Task<string?> Test24Async()
        {
            return Task.FromResult(
                _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                    HttpContext,
                    c => c.Test24Async()));
        }

        [HttpGet]
        public string? Test25()
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test25(),
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
                c => c.Test26(value),
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
                c => c.Test27());
        }

        [HttpGet]
        public string? Test28([TestBindingSource("Header")] string value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test28(value));
        }

        [HttpGet]
        public string? Test29([TestModelNameProvider("value2")] int value)
        {
            return _uriGenerator.GetUriByExpression<AttributeRoutingController>(
                HttpContext,
                c => c.Test29(value));
        }
    }
}
