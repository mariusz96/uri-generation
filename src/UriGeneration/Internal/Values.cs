using Microsoft.AspNetCore.Routing;

namespace UriGeneration.Internal
{
    internal class Values
    {
        public string ActionName { get; set; }
        public string ControllerName { get; set; }
        public RouteValueDictionary RouteValues { get; set; }

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
