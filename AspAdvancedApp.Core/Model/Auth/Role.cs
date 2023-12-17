using System.Text.Json.Serialization;
using System.Xml.Serialization;
using AspAdvancedApp.Core.Model._Base;

namespace AspAdvancedApp.Core.Model.Auth;

/// <summary>
/// Роль пользователя в системе
/// </summary>
public class Role : BaseEntity
{
    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Описание
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Связь с пользователями
    /// </summary>
    [JsonIgnore]
    [XmlIgnore]
    public virtual ICollection<User> Users { get; set; }
}