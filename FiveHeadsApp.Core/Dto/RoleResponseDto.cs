namespace FiveHeadsApp.Core.Dto;

/// <summary>
/// Информация о роли пользователя
/// </summary>
public class RoleResponseDto
{
    /// <summary>
    /// Id
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }
}