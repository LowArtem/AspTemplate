using System.ComponentModel.DataAnnotations;

namespace FiveHeadsApp.Core.Dto;

/// <summary>
/// Данные для регистрации пользователяы
/// </summary>
public class RegisterDto
{
    /// <summary>
    /// Почта
    /// </summary>
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
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
    /// Отчество
    /// </summary>
    public string? MiddleName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    [Required]
    public string LastName { get; set; }
}