using System.Security.Cryptography;
using System.Text;

namespace AspAdvancedApp.Core.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Получение хэша строки
    /// </summary>
    /// <param name="s">Строка</param>
    /// <returns></returns>
    public static string Hash(this string s)
    {
        using var md5 = MD5.Create();

        var inputBytes = Encoding.ASCII.GetBytes(s);
        var hashBytes = md5.ComputeHash(inputBytes);

        StringBuilder sb = new();

        foreach (var b in hashBytes)
        {
            sb.Append(b.ToString("X2"));
        }

        return sb.ToString();
    }
}