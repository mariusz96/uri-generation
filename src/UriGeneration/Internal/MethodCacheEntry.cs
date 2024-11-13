using Microsoft.AspNetCore.Mvc.Controllers;
using System.Diagnostics.CodeAnalysis;

namespace UriGeneration.Internal
{
    internal class MethodCacheEntry
    {
        private static readonly MethodCacheEntry InvalidInstance =
            new(
                isValid: false,
                actionDescriptor: null);

        [MemberNotNullWhen(
            true,
            nameof(ActionDescriptor))]
        public bool IsValid { get; }
        public ControllerActionDescriptor? ActionDescriptor { get; }

        private MethodCacheEntry(
            bool isValid,
            ControllerActionDescriptor? actionDescriptor)
        {
            IsValid = isValid;
            ActionDescriptor = actionDescriptor;
        }

        public static MethodCacheEntry Valid(
            ControllerActionDescriptor actionDescriptor)
        {
            return new(
                isValid: true,
                actionDescriptor);
        }

        public static MethodCacheEntry Invalid() => InvalidInstance;
    }
}
