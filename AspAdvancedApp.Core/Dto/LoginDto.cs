using System.ComponentModel.DataAnnotations;

namespace AspAdvancedApp.Core.Dto;

/// <summary>
/// Данные для входа пользователя
/// </summary>
/// <param name="Email">Почта</param>
/// <param name="Password">Пароль</param>
public record LoginDto
(
    [Required]
    [EmailAddress(ErrorMessage = "Invalid email address")]
    string Email,

    [Required] string Password
);