using FiveHeadsApp.Core.Dto;

namespace FiveHeadsApp.Api.Dto.User;

/// <summary>
/// Информация о пользователе
/// </summary>
public class UserResponseDto
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public virtual string? MiddleName { get; set; }

    /// <summary>
    /// Роли пользователя
    /// </summary>
    public virtual List<RoleResponseDto> UserRoles { get; set; } = new();
}