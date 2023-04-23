using System.Text.RegularExpressions;

namespace FiveHeadsApp.Api.Extensions.Api;

public static class StringValidationExtensions
{
    public static bool ValidateAsEmail(this string str)
    {
        var regex = new Regex("^[a-zA-Z\\d_!#$%&'*+/=?`{|}~^.-]+@[a-zA-Z\\d.-]+$");
        return regex.IsMatch(str);
    }

    public static bool ValidateAsPhoneNumber(this string str)
    {
        var regex = new Regex("^\\d{11}$");
        return regex.IsMatch(str);
    }

    public static bool ValidateAsFilePath(this string str)
    {
        var regex = new Regex(@"^([a-zA-Z]\:|\\\\[^\/\\:*?""<>|]+\\[^\/\\:*?""<>|]+)(\\[^\/\\:*?""<>|]+)+(\.[a-zA-Z0-9]+)$");
        return regex.IsMatch(str);
    }

    public static bool ValidateAsUrl(this string str)
    {
        var regex = new Regex(
            @"https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)");
        return regex.IsMatch(str);
    }
}