namespace Core.Helpers
{
    public static class StringHelper
    {
        public static string ToKebabCase(this string str) => string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "-" + x.ToString() : x.ToString())).ToLower();

    }
}
