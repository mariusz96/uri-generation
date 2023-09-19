using UriGeneration.IntegrationTests.WebApplicationFactories;

namespace UriGeneration.IntegrationTests
{
    public class AreasTest
        : IClassFixture<AreasWebApplicationFactory>
    {
        private readonly AreasWebApplicationFactory _factory;

        public AreasTest(AreasWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Works_InArea()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/Area1/Areas/Test1");

            Assert.Equal("http://localhost/Area1/Areas/Action", uri);
        }

        [Fact]
        public async Task Works_InArea_ToAnotherArea()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/Area1/Areas/Test2");

            Assert.Equal("http://localhost/Area2/Areas/Action", uri);
        }

        [Fact]
        public async Task Works_InArea_ToNonArea()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/Area1/Areas/Test3");

            Assert.Equal("http://localhost/Areas/Action", uri);
        }

        [Fact]
        public async Task Works_InNonArea()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/Areas/Test1");

            Assert.Equal("http://localhost/Areas/Action", uri);
        }

        [Fact]
        public async Task Works_InNonArea_ToArea()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/Areas/Test2");

            Assert.Equal("http://localhost/Area1/Areas/Action", uri);
        }
    }
}
