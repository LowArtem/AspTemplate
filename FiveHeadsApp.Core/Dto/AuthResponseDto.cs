namespace FiveHeadsApp.Core.Dto;

/// <summary>
/// Данные ответа на авторизацию
/// </summary>
/// <param name="Email">Почта</param>
/// <param name="AccessToken">Токен</param>
/// <param name="Roles">Список ролей пользователя</param>
public record AuthResponseDto
(
    string Email,
    string AccessToken,
    List<RoleResponseDto> Roles
);