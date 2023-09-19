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

            Assert.Equal("http://localhost/AnotherAttributeRouting/Action", uri);
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
                "/AttributeRouting/Test5?id=1");

            Assert.Equal("http://localhost/AttributeRouting/Test5?id=1", uri);
        }

        [Fact]
        public async Task Works_WithFromQueryParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test6?id=1");

            Assert.Equal("http://localhost/AttributeRouting/Test6?id=1", uri);
        }

        [Fact]
        public async Task Works_WithFromRouteParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test7/1");

            Assert.Equal("http://localhost/AttributeRouting/Test7/1", uri);
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
                Id = 1,
                Name = "Test"
            };

            var response = await client.PostAsJsonAsync(
                "/AttributeRouting/Test9",
                model);
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test9", uri);
        }

        [Fact]
        public async Task Excludes_FromFormParameter()
        {
            var client = _factory.CreateClient();
            var model = new Dictionary<string, string>
            {
                { "Id", "1" },
                { "Name", "Test" }
            };

            var response = await client.PostAsync(
                "/AttributeRouting/Test10",
                new FormUrlEncodedContent(model));
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test10", uri);
        }

        [Fact]
        public async Task Excludes_FromHeaderParameter()
        {
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("apiKey", "Test");

            string uri = await client.GetStringAsync("/AttributeRouting/Test11");

            Assert.Equal("http://localhost/AttributeRouting/Test11", uri);
        }

        [Fact]
        public async Task Excludes_FromServicesParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/AttributeRouting/Test12");

            Assert.Equal("http://localhost/AttributeRouting/Test12", uri);
        }

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
                "/AttributeRouting/Test13",
                content);
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test13", uri);
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
                "/AttributeRouting/Test14",
                content);
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test14", uri);
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
                "/AttributeRouting/Test15",
                content);
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test15", uri);
        }

        [Fact]
        public async Task Excludes_CancellationTokenParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/AttributeRouting/Test16");

            Assert.Equal("http://localhost/AttributeRouting/Test16", uri);
        }

        [Fact]
        public async Task Excludes_IFormCollectionParameter()
        {
            var client = _factory.CreateClient();
            var content = new MultipartFormDataContent
            {
                {
                    new StringContent("1"),
                    "Id"
                },
                {
                    new StringContent("Test"),
                    "Name"
                }
            };

            var response = await client.PostAsync(
                "/AttributeRouting/Test17",
                content);
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/AttributeRouting/Test17", uri);
        }

        [InlineData(
            "/EndpointName1/Test18?endpointName=En1",
            "http://localhost/EndpointName1/Test18?endpointName=En1")]
        [InlineData(
            "/EndpointName1/Test18?endpointName=En2",
            "http://localhost/EndpointName2/Test18?endpointName=En2")]
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

            string uri = await client.GetStringAsync("/AttributeRouting/Test19");

            Assert.Equal("http://localhost/AttributeRouting/Test19", uri);
        }

        [Fact]
        public async Task Passes_LinkOptions()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/AttributeRouting/Test20");

            Assert.Equal("http://localhost/AttributeRouting/Test20/", uri);
        }

        [Fact]
        public async Task Works_WithoutMethodCacheAndCachedExpressionCompiler()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/AttributeRouting/Test21?id=1");

            Assert.Equal("http://localhost/AttributeRouting/Test21?id=1", uri);
        }

        [Fact]
        public async Task Works_WithRouteTemplateProvider()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/TemplateProvider/Test22");

            Assert.Equal("http://localhost/TemplateProvider/Test22", uri);
        }
    }
}
