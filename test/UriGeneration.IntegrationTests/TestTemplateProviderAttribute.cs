using Microsoft.AspNetCore.Mvc.Routing;

namespace UriGeneration.IntegrationTests
{
    public class TestTemplateProvider : Attribute, IRouteTemplateProvider
    {
        public TestTemplateProvider(string template)
        {
            Template = template;
        }

        public string? Template { get; }
        public int? Order { get; }
        public string? Name { get; }
    }
}
