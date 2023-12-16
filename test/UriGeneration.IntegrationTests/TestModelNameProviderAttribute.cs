using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UriGeneration.IntegrationTests
{
    public class TestModelNameProviderAttribute : Attribute, IModelNameProvider
    {
        public TestModelNameProviderAttribute(string name)
        {
            Name = name;
        }

        public string? Name { get; }
    }
}
