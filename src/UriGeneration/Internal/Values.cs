namespace UriGeneration.Internal
{
    internal class Values
    {
        public string ActionName { get; }
        public string ControllerName { get; }
        public ICollection<KeyValuePair<string, object?>> RouteValues { get; }

        public Values(
            string actionName,
            string controllerName,
            ICollection<KeyValuePair<string, object?>> routeValues)
        {
            ActionName = actionName;
            ControllerName = controllerName;
            RouteValues = routeValues;
        }
    }
}
