using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using System.Xml.Serialization;
using AutoMapper.Configuration.Annotations;

namespace FiveHeadsApp.Core.Model._Base;

/// <summary>
/// Базовый класс для сущностей БД
/// </summary>
public class BaseEntity : IEntity
{
    public BaseEntity()
    {
        DateCreate = DateTime.UtcNow;
        DateUpdate = DateCreate;
    }

    /// <inheritdoc />
    [Key]
    public int Id { get; set; }

    /// <inheritdoc />
    [ReadOnly(true)]
    [JsonIgnore]
    [Ignore]
    public DateTime DateCreate { get; set; }

    /// <inheritdoc />
    [ReadOnly(true)]
    [JsonIgnore]
    [Ignore]
    public DateTime DateUpdate { get; set; }

    /// <inheritdoc />
    [JsonIgnore]
    [XmlIgnore]
    public bool IsDelete { get; set; }

    /// <inheritdoc />
    public void UpdateBeforeSave(DateTime now)
    {
        DateUpdate = now;
    }
}