using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UriGeneration.IntegrationTests
{
    public class TestBindingSourceMetadataAttribute
        : Attribute, IBindingSourceMetadata
    {
        public TestBindingSourceMetadataAttribute(string bindingSource)
        {
            BindingSource = (BindingSource)typeof(BindingSource)
                .GetField(bindingSource)!
                .GetValue(null)!;
        }

        public BindingSource? BindingSource { get; set; }
    }
}
