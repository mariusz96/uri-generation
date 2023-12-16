using UriGeneration.IntegrationTests.WebApplicationFactories;

namespace UriGeneration.IntegrationTests
{
    public class ConventionalRoutingTest
        : IClassFixture<CommonWebApplicationFactory>
    {
        private readonly CommonWebApplicationFactory _factory;

        public ConventionalRoutingTest(CommonWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Works_WithDefaultControllerRoute()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync("/ConventionalRouting");

            Assert.Equal("/ConventionalRouting", uri);
        }

        [Fact]
        public async Task Works()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/ConventionalRouting/Test1");

            Assert.Equal("/ConventionalRouting/Test1", uri);
        }

        [Fact]
        public async Task Works_WithAnotherVoidAction()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/ConventionalRouting/Test2");

            Assert.Equal("/ConventionalRouting/VoidAction", uri);
        }

        [Fact]
        public async Task Works_WithAnotherControllerDefaultControllerRoute()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/ConventionalRouting/Test3");

            Assert.Equal("/AnotherConventionalRouting", uri);
        }

        [Fact]
        public async Task Works_WithAnotherController()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/ConventionalRouting/Test4");

            Assert.Equal("/AnotherConventionalRouting/Action", uri);
        }

        [Fact]
        public async Task Works_WithoutHttpContext()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/ConventionalRouting/Test5");

            Assert.Equal("/ConventionalRouting/Test5", uri);
        }

        [Fact]
        public async Task Works_WithParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/ConventionalRouting/Test6?value=1");

            Assert.Equal("/ConventionalRouting/Test6?value=1", uri);
        }

        [Fact]
        public async Task Works_WithCollectionParameter()
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(
                "/ConventionalRouting/Test7?values=1&values=2");

            Assert.Equal("/ConventionalRouting/Test7?values=1&values=2", uri);
        }

        [InlineData(
            "/RouteName1/Test8?routeName=Rn1",
            "/RouteName1/Test8?routeName=Rn1")]
        [InlineData(
            "/RouteName1/Test8?routeName=Rn2",
            "/RouteName2/Test8?routeName=Rn2")]
        [Theory]
        public async Task Works_WithDedicatedConventionalRoute(
            string requestUri,
            string expectedUri)
        {
            var client = _factory.CreateClient();

            string uri = await client.GetStringAsync(requestUri);

            Assert.Equal(expectedUri, uri);
        }
    }
}
