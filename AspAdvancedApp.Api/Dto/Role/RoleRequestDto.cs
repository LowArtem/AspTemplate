namespace AspAdvancedApp.Api.Dto.Role;

/// <summary>
/// Данные для создания роли
/// </summary>
/// <param name="Name">Название</param>
/// <param name="Description">Описание</param>
public record RoleRequestDto(string Name, string Description);