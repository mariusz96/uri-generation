using UriGeneration.IntegrationTests.WebApplicationFactories;

namespace UriGeneration.IntegrationTests
{
    public class NamespaceTest
        : IClassFixture<NamespaceWebApplicationFactory>
    {
        private readonly NamespaceWebApplicationFactory _factory;

        public NamespaceTest(NamespaceWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Works_WithControllerModelConvention()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "Controllers/Namespace/Test1");

            Assert.Equal("http://localhost/Controllers/Namespace/Test1", uri);
        }
    }
}
