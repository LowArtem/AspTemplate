using System.Data;
using System.Linq.Expressions;
using AspAdvancedApp.Core.Model._Base;
using AspAdvancedApp.Core.Repositories;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MoreLinq.Extensions;

namespace AspAdvancedApp.Data.Repositories;

/// <summary>
/// Базовый репозиторий
/// </summary>
/// <typeparam name="TEntity">модель</typeparam>
/// <typeparam name="TContext">контекст</typeparam>
public class EfCoreRepository<TEntity, TContext> : IEfCoreRepository<TEntity>
    where TEntity : class, IEntity
    where TContext : DbContext
{
    protected readonly TContext _db;
    private readonly ILogger<IEfCoreRepository<TEntity>> _logger;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="db">контекст бд</param>
    /// <param name="logger">логирование</param>
    protected EfCoreRepository(TContext db, ILogger<IEfCoreRepository<TEntity>> logger)
    {
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Обновление модели перед записью в БД
    /// </summary>
    /// <param name="model">Модель</param>
    /// <param name="nowUtc">Текущее время</param>
    /// <returns>обновленная запись</returns>
    private static TEntity UpdateEntityBeforeSave(TEntity model, DateTime nowUtc)
    {
        model.UpdateBeforeSave(nowUtc);
        return model;
    }

    /// <summary>
    /// Обновление модели перед записью в БД
    /// </summary>
    /// <param name="model">Модель</param>
    /// <returns>обновленная запись</returns>
    private static TEntity UpdateEntityBeforeSave(TEntity model)
    {
        return UpdateEntityBeforeSave(model, DateTime.UtcNow);
    }

    /// <summary>
    /// Обновление моделей перед записью в БД
    /// </summary>
    /// <param name="models">Модели</param>
    /// <returns>обновлённые модели</returns>
    private static IEnumerable<TEntity> UpdateEntityBeforeSave(IEnumerable<TEntity> models)
    {
        var now = DateTime.UtcNow;
        return models.Select(p => UpdateEntityBeforeSave(p, now));
    }

    /// <inheritdoc />
    public virtual void Add(TEntity model)
    {
        _db.Add(model);
    }

    /// <inheritdoc />
    public virtual void AddRange(IEnumerable<TEntity> models)
    {
        _db.AddRange(models);
    }

    /// <inheritdoc />
    public virtual void Update(TEntity model)
    {
        _db.Update(UpdateEntityBeforeSave(model));
    }

    /// <inheritdoc />
    public virtual void UpdateRange(IEnumerable<TEntity> models)
    {
        _db.UpdateRange(UpdateEntityBeforeSave(models));
    }

    /// <inheritdoc />
    public async Task AddRangeAsync(IEnumerable<TEntity> models)
    {
        await _db.AddRangeAsync(models);
    }

    /// <inheritdoc />
    public virtual void Remove(TEntity? model)
    {
        if (model == null) return;

        model.IsDelete = true;
        _db.Update(UpdateEntityBeforeSave(model));
    }

    /// <inheritdoc />
    public virtual void Remove(int id)
    {
        var model = _db.Set<TEntity>()
            .AsNoTracking()
            .FirstOrDefault(p => p.Id == id);

        Remove(model);
    }

    /// <inheritdoc />
    public virtual void RemoveRange(IEnumerable<TEntity> models)
    {
        models.ForEach(p => p.IsDelete = true);

        _db.UpdateRange(UpdateEntityBeforeSave(models));
    }

    /// <inheritdoc />
    public virtual void RemoveRange(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return;

        RemoveRange(_db.Set<TEntity>().Where(p => ids.Contains(p.Id)));
    }

    /// <inheritdoc />
    public virtual void Delete(TEntity model)
    {
        _db.Remove(model);
    }

    /// <inheritdoc />
    public virtual void DeleteRange(IEnumerable<TEntity> models)
    {
        _db.RemoveRange(models);
    }

    /// <inheritdoc />
    public virtual void DeleteRange(IEnumerable<int> ids)
    {
        if (ids == null || !ids.Any())
            return;

        DeleteRange(_db.Set<TEntity>().Where(p => ids.Contains(p.Id)));
    }

    /// <inheritdoc />
    public virtual IQueryable<TEntity> GetListQuery()
    {
        return _db.Set<TEntity>().AsNoTracking().Where(p => !p.IsDelete).AsQueryable();
    }

    /// <inheritdoc />
    public virtual IQueryable<TEntity> GetListQueryWithDeleted()
    {
        return _db.Set<TEntity>().AsNoTracking().AsQueryable();
    }

    /// <inheritdoc />
    public virtual List<TEntity> GetList()
    {
        return _db.Set<TEntity>().AsNoTracking().Where(p => !p.IsDelete).ToList();
    }

    /// <inheritdoc />
    public virtual IEnumerable<TEntity> GetListWithDeleted()
    {
        return _db.Set<TEntity>().AsNoTracking().AsQueryable();
    }

    /// <inheritdoc />
    public virtual bool Any(Expression<Func<TEntity, bool>> func)
    {
        return GetListQuery().Any(func);
    }

    /// <inheritdoc />
    public virtual TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> func)
    {
        return GetListQuery().FirstOrDefault(func);
    }

    /// <inheritdoc />
    public virtual TEntity? Get(int id)
    {
        return GetListQuery().FirstOrDefault(p => p.Id == id);
    }

    /// <inheritdoc />
    public int SaveChanges()
    {
        return SaveChangesAsync().Result;
    }

    /// <inheritdoc />
    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _db.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(message: $"При сохранении изменений произошла ошибка: {ex}");
            throw;
        }
    }

    /// <inheritdoc />
    public int Count()
    {
        return GetListQuery().Count();
    }

    /// <inheritdoc />
    public IEnumerable<T> RawQuerySql<T>(string sql, object? param = null)
    {
        if (param == null)
            _logger.LogWarning("Используйте параметр \"param\" для предотвращения SQL-Injections");

        var conn = _db.Database.GetDbConnection();

        if (conn.State == ConnectionState.Closed)
        {
            conn.Open();
        }

        return conn.Query<T>(sql, param: param);
    }

    /// <inheritdoc />
    public T RawQuerySingleSql<T>(string sql, object? param = null)
    {
        if (param == null)
            _logger.LogWarning("Используйте параметр \"param\" для предотвращения SQL-Injections");

        var conn = _db.Database.GetDbConnection();

        if (conn.State == ConnectionState.Closed)
        {
            conn.Open();
        }

        return conn.QueryFirstOrDefault<T>(sql, param: param);
    }
}