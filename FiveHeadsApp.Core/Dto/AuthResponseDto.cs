namespace FiveHeadsApp.Core.Dto;

/// <summary>
/// Данные ответа на авторизацию
/// </summary>
public class AuthResponseDto
{
    /// <summary>
    /// Почта
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Токен
    /// </summary>
    public string AccessToken { get; set; }

    /// <summary>
    /// Список ролей пользователя
    /// </summary>
    public List<RoleResponseDto> Roles { get; set; }
}