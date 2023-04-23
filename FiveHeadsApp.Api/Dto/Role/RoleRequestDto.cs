namespace FiveHeadsApp.Api.Dto.Role;

/// <summary>
/// Данные для создания роли
/// </summary>
public class RoleRequestDto
{
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }
}