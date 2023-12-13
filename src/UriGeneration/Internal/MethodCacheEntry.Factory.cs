using Microsoft.AspNetCore.Mvc.Controllers;

namespace UriGeneration.Internal
{
    internal partial class MethodCacheEntry
    {
        private static readonly MethodCacheEntry InvalidInstance =
            new(isValid: false);

        public static MethodCacheEntry Valid(
            ControllerActionDescriptor actionDescriptor) =>
            new(isValid: true, actionDescriptor);

        public static MethodCacheEntry Invalid() => InvalidInstance;
    }
}
