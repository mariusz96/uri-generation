using System.Net.Http.Json;
using System.Text;
using UriGeneration.IntegrationTests.Models;
using UriGeneration.IntegrationTests.WebApplicationFactories;

namespace UriGeneration.IntegrationTests
{
    public class AttributeRoutingTest
        : IClassFixture<CommonWebApplicationFactory>
    {
        private readonly CommonWebApplicationFactory _factory;

        public AttributeRoutingTest(CommonWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Works()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/AttributeRouting/Test1");

            Assert.Equal("http://localhost/AttributeRouting/Test1", uri);
        }

        [Fact]
        public async Task Works_WithAnotherVoidAction()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/AttributeRouting/Test2");

            Assert.Equal("http://localhost/AttributeRouting/VoidAction", uri);
        }

        [Fact]
        public async Task Works_WithAnotherController()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/AttributeRouting/Test3");

            Assert.Equal(
                "http://localhost/AnotherAttributeRouting/Action",
                uri);
        }

        [Fact]
        public async Task Works_WithoutHttpContext()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/AttributeRouting/Test4");

            Assert.Equal("http://localhost/AttributeRouting/Test4", uri);
        }

        [Fact]
        public async Task Works_WithParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test5?value=1");

            Assert.Equal("http://localhost/AttributeRouting/Test5?value=1", uri);
        }

        [Fact]
        public async Task Works_WithCollectionParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test6?values=1&values=2");

            Assert.Equal(
                "http://localhost/AttributeRouting/Test6?values=1&values=2",
                uri);
        }

        [Fact]
        public async Task Works_WithFromQueryParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test7?value=1");

            Assert.Equal("http://localhost/AttributeRouting/Test7?value=1", uri);
        }

        [Fact]
        public async Task Works_WithFromQueryParameterWithModelName()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test8?value2=1");

            Assert.Equal("http://localhost/AttributeRouting/Test8?value2=1", uri);
        }

        [Fact]
        public async Task Works_WithRouteParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test9/1");

            Assert.Equal("http://localhost/AttributeRouting/Test9/1", uri);
        }

        [Fact]
        public async Task Works_WithFromRouteParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test10/1");

            Assert.Equal("http://localhost/AttributeRouting/Test10/1", uri);
        }

        [Fact]
        public async Task Works_WithFromRouteParameterWithModelName()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test11/1");

            Assert.Equal("http://localhost/AttributeRouting/Test11/1", uri);
        }

        [Fact]
        public async Task Works_WithActionNameAttribute()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/ActionName");

            Assert.Equal("http://localhost/AttributeRouting/ActionName", uri);
        }

        [Fact]
        public async Task Excludes_FromBodyParameter()
        {
            var client = _factory.CreateClient();
            var model = new TestModel
            {
                Value = "Test"
            };

            var response = await client.PostAsJsonAsync(
                "/AttributeRouting/Test13",
                model);
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test13", uri);
        }

        [Fact]
        public async Task Excludes_FromFormParameter()
        {
            var client = _factory.CreateClient();
            var model = new Dictionary<string, string>
            {
                { "Value", "Test" }
            };

            var response = await client.PostAsync(
                "/AttributeRouting/Test14",
                new FormUrlEncodedContent(model));
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test14", uri);
        }

        [Fact]
        public async Task Excludes_FromHeaderParameter()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("value", "Test");

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test15");

            Assert.Equal("http://localhost/AttributeRouting/Test15", uri);
        }

        [Fact]
        public async Task Excludes_FromServicesParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test16");

            Assert.Equal("http://localhost/AttributeRouting/Test16", uri);
        }

#if NET8_0_OR_GREATER
        [Fact]
        public async Task Excludes_FromKeyedServicesParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test17");

            Assert.Equal("http://localhost/AttributeRouting/Test17", uri);
        }
#endif

        [Fact]
        public async Task Excludes_IFormFileParameter()
        {
            var client = _factory.CreateClient();
            var content = new MultipartFormDataContent
            {
                {
                    new ByteArrayContent(Encoding.UTF8.GetBytes("Test")),
                    "file",
                    "file.txt"
                }
            };

            var response = await client.PostAsync(
                "/AttributeRouting/Test18",
                content);
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test18", uri);
        }

        [Fact]
        public async Task Excludes_IFormFileCollectionParameter()
        {
            var client = _factory.CreateClient();
            var content = new MultipartFormDataContent
            {
                {
                    new ByteArrayContent(Encoding.UTF8.GetBytes("Test1")),
                    "files",
                    "file1.txt"
                },
                {
                    new ByteArrayContent(Encoding.UTF8.GetBytes("Test2")),
                    "files",
                    "file2.txt"
                }
            };

            var response = await client.PostAsync(
                "/AttributeRouting/Test19",
                content);
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test19", uri);
        }

        [Fact]
        public async Task Excludes_EnumerableOfIFormFileParameter()
        {
            var client = _factory.CreateClient();
            var content = new MultipartFormDataContent
            {
                {
                    new ByteArrayContent(Encoding.UTF8.GetBytes("Test1")),
                    "files",
                    "file1.txt"
                },
                {
                    new ByteArrayContent(Encoding.UTF8.GetBytes("Test2")),
                    "files",
                    "file2.txt"
                }
            };

            var response = await client.PostAsync(
                "/AttributeRouting/Test20",
                content);
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test20", uri);
        }

        [Fact]
        public async Task Excludes_CancellationTokenParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test21");

            Assert.Equal("http://localhost/AttributeRouting/Test21", uri);
        }

        [Fact]
        public async Task Excludes_IFormCollectionParameter()
        {
            var client = _factory.CreateClient();
            var content = new MultipartFormDataContent
            {
                {
                    new StringContent("Test"),
                    "Value"
                }
            };

            var response = await client.PostAsync(
                "/AttributeRouting/Test22",
                content);
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test22", uri);
        }

        [InlineData(
            "/EndpointName1/Test23?endpointName=En1",
            "http://localhost/EndpointName1/Test23?endpointName=En1")]
        [InlineData(
            "/EndpointName1/Test23?endpointName=En2",
            "http://localhost/EndpointName2/Test23?endpointName=En2")]
        [Theory]
        public async Task Works_WithEndpointName(
            string requestUri,
            string expectedUri)
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(requestUri);

            Assert.Equal(expectedUri, uri);
        }

        [Fact]
        public async Task Works_WithAsyncSuffix()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test24");

            Assert.Equal("http://localhost/AttributeRouting/Test24", uri);
        }

        [Fact]
        public async Task Passes_LinkOptions()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test25");

            Assert.Equal("http://localhost/AttributeRouting/Test25/", uri);
        }

        [Fact]
        public async Task Works_WithoutMethodCacheAndCachedExpressionCompiler()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test26?value=1");

            Assert.Equal("http://localhost/AttributeRouting/Test26?value=1", uri);
        }

        [Fact]
        public async Task Works_WithRouteTemplateProvider()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/TemplateProvider/Test27");

            Assert.Equal("http://localhost/TemplateProvider/Test27", uri);
        }

        [Fact]
        public async Task Works_WithBindingSourceMetadata()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("value", "Test");

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test28");

            Assert.Equal("http://localhost/AttributeRouting/Test28", uri);
        }

        [Fact]
        public async Task Works_WithModelNameProvider()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test29?value2=1");

            Assert.Equal("http://localhost/AttributeRouting/Test29?value2=1", uri);
        }
    }
}
