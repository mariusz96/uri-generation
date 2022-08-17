namespace UriGeneration.Internal
{
    internal static class StringExtensions
    {
        public static string RemoveSuffix(this string value, string suffix)
        {
            if (!value.EndsWith(suffix))
            {
                return value;
            }

            int index = value.LastIndexOf(suffix);
            return value.Remove(index);
        }
    }
}
