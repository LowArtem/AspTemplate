using System.Text;
using AspAdvancedApp.Core.Model.Auth;
using Microsoft.IdentityModel.Tokens;

namespace AspAdvancedApp.Api.Services.Auth;

/// <summary>
/// Класс для работы с JWT
/// </summary>
public class JwtService : IJwtService
{
    /// <summary>
    /// Ключ для шифрования
    /// </summary>
    public string Key { get; }
    
    /// <summary>
    /// Издатель токена
    /// </summary>
    public string Issuer { get; }
    
    /// <summary>
    /// Потребитель токена
    /// </summary>
    public string Audience { get; }
    
    /// <summary>
    /// Время жизни токена в минутах
    /// </summary>
    public int Lifetime { get; } = 60 * 24 * 365;

    public JwtService(IConfiguration configuration)
    {
        Key = configuration.GetValue<string>("JWT:Key") ?? throw new InvalidOperationException("Jwt key is not set");
        Issuer = configuration.GetValue<string>("JWT:Issuer") ?? throw new InvalidOperationException("Jwt issuer is not set");
        Audience = configuration.GetValue<string>("JWT:Audience") ?? throw new InvalidOperationException("Jwt audience is not set");
    }

    /// <summary>
    /// Получить ключ
    /// </summary>
    /// <returns></returns>
    public SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
    }
}