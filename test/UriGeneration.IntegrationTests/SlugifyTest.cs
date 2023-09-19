using UriGeneration.IntegrationTests.WebApplicationFactories;

namespace UriGeneration.IntegrationTests
{
    public class SlugifyTest
        : IClassFixture<SlugifyWebApplicationFactory>
    {
        private readonly SlugifyWebApplicationFactory _factory;

        public SlugifyTest(SlugifyWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async void Works_WithOutboundParameterTransformer()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/slugify/slug-test1");

            Assert.Equal("http://localhost/slugify/slug-test1", uri);
        }
    }
}
