using System.Net;
using UriGeneration.IntegrationTests.WebApplicationFactories;

namespace UriGeneration.IntegrationTests
{
    public class BoundPropertiesTest
        : IClassFixture<CommonWebApplicationFactory>
    {
        private readonly CommonWebApplicationFactory _factory;

        public BoundPropertiesTest(CommonWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task DoesNotValidate_Property()
        {
            var client = _factory.CreateClient();

            var response = await client.PostAsync(
                "/BoundProperties1/Test1",
                null);
            string uri = await response.Content.ReadAsStringAsync();

            Assert.Equal("http://localhost/BoundProperties1/Test1", uri);
        }

        [Fact]
        public async Task Validates_BindPropertyProperty()
        {
            var client = _factory.CreateClient();
            var model = new Dictionary<string, string>
            {
                { "Value", "Test" }
            };

            var response = await client.PostAsync(
                "/BoundProperties2/Test1",
                new FormUrlEncodedContent(model));
            var statusCode = response.StatusCode;

            Assert.Equal(HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Validates_BindPropertiesProperty()
        {
            var client = _factory.CreateClient();
            var model = new Dictionary<string, string>
            {
                { "Value", "Test" }
            };

            var response = await client.PostAsync(
                "/BoundProperties3/Test1",
                new FormUrlEncodedContent(model));
            var statusCode = response.StatusCode;

            Assert.Equal(HttpStatusCode.NoContent, statusCode);
        }
    }
}
