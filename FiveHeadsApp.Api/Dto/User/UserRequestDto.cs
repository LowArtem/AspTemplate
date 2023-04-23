using System.ComponentModel.DataAnnotations;

namespace FiveHeadsApp.Api.Dto.User;

/// <summary>
/// Данные для создания пользователя
/// </summary>
public class UserRequestDto
{
    /// <summary>
    /// Электронная почта
    /// </summary>
    [Required]
    [EmailAddress]
    public string Email { get; set; }

    /// <summary>
    /// Пароль
    /// </summary>
    [Required]
    public string Password { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    [Required]
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    [Required]
    public string LastName { get; set; }

    /// <summary>
    /// Отчество (при наличии)
    /// </summary>
    public virtual string? MiddleName { get; set; }
}