using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UriGeneration.IntegrationTests
{
    public class TestBindingSourceAttribute : Attribute, IBindingSourceMetadata
    {
        public TestBindingSourceAttribute(string bindingSource)
        {
            BindingSource = bindingSource switch
            {
                "Body" => BindingSource.Body,
                "Custom" => BindingSource.Custom,
                "Form" => BindingSource.Form,
                "FormFile" => BindingSource.FormFile,
                "Header" => BindingSource.Header,
                "ModelBinding" => BindingSource.ModelBinding,
                "Path" => BindingSource.Path,
                "Query" => BindingSource.Query,
                "Services" => BindingSource.Services,
                "Special" => BindingSource.Special,
                _ => null
            };
        }

        public BindingSource? BindingSource { get; }
    }
}
