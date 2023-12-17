using System.ComponentModel.DataAnnotations;

namespace AspAdvancedApp.Core.Dto;

/// <summary>
///  Данные для регистрации пользователяы
/// </summary>
/// <param name="Email">Почта</param>
/// <param name="Password">Пароль</param>
/// <param name="FirstName">Имя</param>
/// <param name="MiddleName">Отчество</param>
/// <param name="LastName">Фамилия</param>
public record RegisterDto
(
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    string Email,

    [Required] string Password,
    [Required] string FirstName,
    [Required] string LastName,
    string? MiddleName
);