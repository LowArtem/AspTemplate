using Microsoft.IdentityModel.Tokens;

namespace AspAdvancedApp.Core.Model.Auth;

/// <summary>
/// Для работы с JWT
/// </summary>
public interface IJwtService
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
    public int Lifetime { get; }

    /// <summary>
    /// Получить ключ
    /// </summary>
    /// <returns></returns>
    public SymmetricSecurityKey GetSymmetricSecurityKey();
}