using System.ComponentModel.DataAnnotations;

namespace FiveHeadsApp.Api.Dto.User;

/// <summary>
/// Данные для создания пользователя
/// </summary>
/// <param name="Email">Электронная почта</param>
/// <param name="Password">Пароль</param>
/// <param name="FirstName">Имя</param>
/// <param name="LastName">Фамилия</param>
/// <param name="MiddleName">Отчество (при наличии)</param>
public record UserRequestDto
(
    [Required] 
    [EmailAddress(ErrorMessage = "Invalid email address")]
    string Email,
    
    [Required] string Password,
    [Required] string FirstName,
    [Required] string LastName, 
    string? MiddleName
);