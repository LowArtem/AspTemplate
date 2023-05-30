using FiveHeadsApp.Core.Dto;

namespace FiveHeadsApp.Api.Dto.User;

/// <summary>
/// Информация о пользователе
/// </summary>
/// <param name="Id">Id</param>
/// <param name="Email">Электронная почта</param>
/// <param name="FirstName">Имя</param>
/// <param name="LastName">Фамилия</param>
/// <param name="MiddleName">Отчество</param>
/// <param name="UserRoles">Роли пользователя</param>
public record UserResponseDto
(
    int Id, 
    string Email, 
    string FirstName, 
    string LastName, 
    string? MiddleName, 
    List<RoleResponseDto> UserRoles
);