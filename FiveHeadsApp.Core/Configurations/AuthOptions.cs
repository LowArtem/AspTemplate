using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace FiveHeadsApp.Core.Configurations;

public static class AuthOptions
{
    /// <summary>
    /// Издатель токена
    /// </summary>
    public const string ISSUER = "Backend";

    /// <summary>
    /// Потребитель токена
    /// </summary>
    public const string AUDIENCE = "API";

    /// <summary>
    /// Ключ для шифрации
    /// </summary>
    const string KEY = "234k5s^%&*@&$ogh23&GAF43LH3JG6jodagfgzi,GasfhASDFijbfahjbarfvdfgjhl;hSOBhskjf$";

    /// <summary>
    /// Время жизни токена в минутах
    /// </summary>
    public const int LIFETIME = 60 * 24 * 365;

    /// <summary>
    /// Получить ключ
    /// </summary>
    /// <returns></returns>
    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(KEY));
    }
}