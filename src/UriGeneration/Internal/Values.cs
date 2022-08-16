using Microsoft.AspNetCore.Routing;

namespace UriGeneration.Internal
{
    internal class Values
    {
        public string ActionName { get; }
        public string ControllerName { get; }
        public RouteValueDictionary RouteValues { get; }

        public Values(
            string actionName,
            string controllerName,
            RouteValueDictionary routeValues)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            RouteValues = routeValues;
        }
    }
}
