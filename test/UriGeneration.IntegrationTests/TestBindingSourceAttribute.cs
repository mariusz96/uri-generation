using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UriGeneration.IntegrationTests
{
    public class TestBindingSourceAttribute : Attribute, IBindingSourceMetadata
    {
        public TestBindingSourceAttribute(string bindingSource)
        {
            BindingSource = bindingSource switch
            {
                "Header" => BindingSource.Header,
                _ => null
            };
        }

        public BindingSource? BindingSource { get; }
    }
}
