namespace AbsurdReplies
{
    public static class StringExtensions
    {
        public static bool HasValue(this string s) => !string.IsNullOrWhiteSpace(s);
    }
}