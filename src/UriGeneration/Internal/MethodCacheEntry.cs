using Microsoft.AspNetCore.Mvc.Controllers;
using System.Diagnostics.CodeAnalysis;

namespace UriGeneration.Internal
{
    internal partial class MethodCacheEntry
    {
        [MemberNotNullWhen(
            true,
            nameof(ActionDescriptor))]
        public bool IsValid { get; }
        public ControllerActionDescriptor? ActionDescriptor { get; }

        private MethodCacheEntry(bool isValid)
        {
            IsValid = isValid;
        }

        private MethodCacheEntry(
            bool isValid,
            ControllerActionDescriptor actionDescriptor)
        {
            IsValid = isValid;
            ActionDescriptor = actionDescriptor;
        }
    }
}
