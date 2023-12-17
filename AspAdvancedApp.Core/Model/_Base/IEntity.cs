namespace AspAdvancedApp.Core.Model._Base;

/// <summary>
/// Интерфейс сущности БД
/// </summary>
public interface IEntity
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Дата создания записи
    /// </summary>
    public DateTime DateCreate { get; set; }

    /// <summary>
    /// Дата обновления записи
    /// </summary>
    public DateTime DateUpdate { get; set; }

    /// <summary>
    /// Флаг удаления
    /// </summary>
    bool IsDelete { get; set; }

    /// <summary>
    /// Обновление дат перед сохранением в БД
    /// </summary>
    /// <param name="now">текущая дата</param>
    public void UpdateBeforeSave(DateTime now);
}