namespace FiveHeadsApp.Core.Dto;

/// <summary>
/// Информация о роли пользователя
/// </summary>
/// <param name="Id">Id</param>
/// <param name="Name">Название</param>
/// <param name="Description">Описание</param>
public record RoleResponseDto(int Id, string Name, string Description);