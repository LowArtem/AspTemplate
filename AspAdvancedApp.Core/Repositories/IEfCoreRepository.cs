using System.Linq.Expressions;
using AspAdvancedApp.Core.Model._Base;

namespace AspAdvancedApp.Core.Repositories;

/// <summary>
/// Интерфейс репозитория
/// </summary>
/// <typeparam name="TEntity">модель</typeparam>
public interface IEfCoreRepository<TEntity>
    where TEntity : IEntity
{
    /// <summary>
    /// Добавить
    /// </summary>
    /// <param name="model">добавляемый объект</param>
    void Add(TEntity model);

    /// <summary>
    /// Добавить несколько
    /// </summary>
    /// <param name="models">коллекция добавляемых объектов</param>
    void AddRange(IEnumerable<TEntity> models);

    /// <summary>
    /// Обновить
    /// </summary>
    /// <param name="model">обновляемый объект</param>
    void Update(TEntity model);

    /// <summary>
    /// Обновить несколько
    /// </summary>
    /// <param name="models">коллекция обновляемых объектов</param>
    void UpdateRange(IEnumerable<TEntity> models);

    /// <summary>
    /// Добавить несколько асинхронно
    /// </summary>
    /// <param name="models">коллекция добавляемых объектов</param>
    /// <returns></returns>
    Task AddRangeAsync(IEnumerable<TEntity> models);

    /// <summary>
    /// Удалить
    /// </summary>
    /// <param name="model">удаляемый объект</param>
    void Remove(TEntity model);

    /// <summary>
    /// Удалить
    /// </summary>
    /// <param name="id">ID удаляемого объекта</param>
    void Remove(int id);

    /// <summary>
    /// Удалить несколько
    /// </summary>
    /// <param name="models">коллекция удаляемых объектов</param>
    void RemoveRange(IEnumerable<TEntity> models);

    /// <summary>
    /// Удалить несколько
    /// </summary>
    /// <param name="ids">коллекция идентификаторов удаляемых объектов</param>
    void RemoveRange(IEnumerable<int> ids);

    /// <summary>
    /// Удалить безвозвратно
    /// </summary>
    /// <param name="model">удаляемый объект</param>
    void Delete(TEntity model);

    /// <summary>
    /// Удалить несколько безвозвратно
    /// </summary>
    /// <param name="models">коллекция удаляемых объектов</param>
    void DeleteRange(IEnumerable<TEntity> models);

    /// <summary>
    /// Удалить несколько безвозвратно
    /// </summary>
    /// <param name="ids">коллекция идентификаторов удаляемых объектов</param>
    void DeleteRange(IEnumerable<int> ids);

    /// <summary>
    /// Получить коллекцию
    /// </summary>
    /// <returns>коллекция объектов</returns>
    IQueryable<TEntity> GetListQuery();

    /// <summary>
    /// Получить коллекцию с удалёнными объектами
    /// </summary>
    /// <returns>коллекция объектов</returns>
    IQueryable<TEntity> GetListQueryWithDeleted();

    /// <summary>
    /// Получить список
    /// </summary>
    /// <returns>список всех объектов</returns>
    List<TEntity> GetList();

    /// <summary>
    /// Получить список с удалёнными объектами
    /// </summary>
    /// <returns>список всех объектов</returns>
    IEnumerable<TEntity> GetListWithDeleted();

    /// <summary>
    /// Проверяет существование хотя бы одного элемента в последовательности
    /// </summary>
    /// <param name="func">условие</param>
    /// <returns>существует ли хотя бы один такой элемент</returns>
    bool Any(Expression<Func<TEntity, bool>> func);

    /// <summary>
    /// Возвращает первый элемент последовательности или значение по умолчанию, если ни одного элемента не найдено
    /// </summary>
    /// <param name="func">условие</param>
    /// <returns>элемент последовательности (или значение по умолчанию)</returns>
    TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> func);

    /// <summary>
    /// Получение записи по идентификатору или значение по умолчанию, если такого элемента не найдено
    /// </summary>
    /// <param name="id">ID объекта</param>
    /// <returns>объект</returns>
    TEntity? Get(int id);

    /// <summary>
    /// Сохранение изменений
    /// </summary>
    /// <returns>количество применённых изменений</returns>
    int SaveChanges();

    /// <summary>
    /// Сохранение изменений асинхронно
    /// </summary>
    /// <returns>количество применённых изменений</returns>
    Task<int> SaveChangesAsync();

    /// <summary>
    /// Получение количества записей в бд
    /// </summary>
    /// <returns>количество записей в бд</returns>
    int Count();

    /// <summary>
    /// Получение записей из БД из SQL
    /// </summary>
    /// <param name="sql">SQL запрос</param>
    /// <param name="param">Параметры запроса - предотвращают SQL Injections</param>
    /// <typeparam name="T">Тип возращаемых записей</typeparam>
    /// <returns>Список записей из БД</returns>
    public IEnumerable<T> RawQuerySql<T>(string sql, object? param = null);

    /// <summary>
    /// Получение записи из БД из SQL
    /// </summary>
    /// <param name="sql">SQL запрос</param>
    /// <param name="param">Параметры запроса - предотвращают SQL Injections</param>
    /// <typeparam name="T">Тип возращаемой записи</typeparam>
    /// <returns>Запись из БД</returns>
    public T RawQuerySingleSql<T>(string sql, object? param = null);
}