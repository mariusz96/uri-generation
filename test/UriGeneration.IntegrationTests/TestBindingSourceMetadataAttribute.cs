using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UriGeneration.IntegrationTests
{
    public class TestBindingSourceMetadataAttribute
        : Attribute, IBindingSourceMetadata
    {
        public TestBindingSourceMetadataAttribute(string bindingSource)
        {
            BindingSource = bindingSource switch
            {
                "Header" => BindingSource.Header,
                _ => throw new ArgumentOutOfRangeException(nameof(bindingSource))
            };
        }

        public BindingSource? BindingSource { get; set; }
    }
}
