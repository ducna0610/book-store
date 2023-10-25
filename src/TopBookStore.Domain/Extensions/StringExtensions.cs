using System.Text;
using Microsoft.Extensions.Logging;

namespace TopBookStore.Domain.Extensions;

public static class StringExtensions
{
    public static string Slug(this string str)
    {
        StringBuilder sb = new();
        foreach (char c in str)
        {
            if (!char.IsPunctuation(c) || c == '-')
            {
                sb.Append(c);
            }
        }
        return sb.ToString().Replace(' ', '-').ToLower();
    }

    public static bool EqualsNoCase(this string str, string strToCompare) =>
        str.ToLower() == strToCompare.ToLower();

    public static int SafeToInt(this string str)
    {
        return int.TryParse(str, out int value) ? value : 0;
    }

    public static string Capitalize(this string str) =>
        // str?.Substring(0, 1)?.ToUpper() + str?.Substring(1)?.ToLower();
        str?[..1]?.ToUpper() + str?[1..].ToLower();
}