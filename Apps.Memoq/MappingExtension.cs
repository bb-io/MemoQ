namespace Apps.Memoq;

public static class MappingExtension
{
    public static string[] ToArray(this string source)
    {
        char[] separators = new[] { ' ', ',', ';' };
        return source.Split(separators, StringSplitOptions.RemoveEmptyEntries);
    }
}