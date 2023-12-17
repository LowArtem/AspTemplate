using System.Text.Json.Serialization;
using System.Xml.Serialization;
using AspAdvancedApp.Core.Model._Base;

namespace AspAdvancedApp.Core.Model.Auth;

/// <summary>
/// Пользователь
/// </summary>
public class User : BaseEntity
{
    /// <summary>
    /// Электронная почта
    /// </summary>
    public string Email { get; set; }

    /// <summary>
    /// ХЭШ пароля
    /// </summary>
    [JsonIgnore]
    [XmlIgnore]
    public string PasswordHash { get; set; }

    /// <summary>
    /// Имя
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Фамилия
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Отчество
    /// </summary>
    public virtual string? MiddleName { get; set; }

    /// <summary>
    /// Роли пользователя
    /// </summary>
    [JsonIgnore]
    public virtual ICollection<Role> UserRoles { get; set; }
}