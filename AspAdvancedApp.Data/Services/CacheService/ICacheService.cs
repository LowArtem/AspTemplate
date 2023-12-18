namespace AspAdvancedApp.Data.Services.CacheService;

/// <summary>
/// Сервис управления КЭШом
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Получение значения из КЭШа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="token">Токен отмены операции</param>
    /// <returns>Значение из КЭШа</returns>
    Task<string?> GetAsync(string key, CancellationToken token = default);

    /// <summary>
    /// Получение значения из КЭШа
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="token">Токен отмены операции</param>
    /// <returns>Значение из КЭШа</returns>
    Task<T> GetAsync<T>(string key, CancellationToken token = default);

    /// <summary>
    /// Сохранение значение в КЭШе
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="value">Сериализованное значение для сохранения в КЭШе</param>
    /// <param name="expiration">Срок жизни записи в КЭШе</param>
    /// <param name="token">Токен отмены операции</param>
    /// <returns></returns>
    Task SetAsync(string key,
        string value,
        TimeSpan? expiration = null,
        CancellationToken token = default);


    /// <summary>
    /// Сохранение значение в КЭШе
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="value">Сериализованное значение для сохранения в КЭШе</param>
    /// <param name="expiration">Срок жизни записи в КЭШе</param>
    /// <param name="token">Токен отмены операции</param>
    /// <returns></returns>
    Task SetAsync<T>(string key,
        T value,
        TimeSpan? expiration = null,
        CancellationToken token = default);


    /// <summary>
    /// Получение значение из КЭШа или его вычисление
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="expiration">Срок жизни записи в КЭШе</param>
    /// <param name="getDataFunc">Функция вычисления значения. Выполняется в случае если записи по ключу нет в КЭШе и в результате выполнения сохраняется в КЭШ.</param>
    /// <param name="token">Токен отмены операции</param>
    /// <typeparam name="T">Тип возращаемый функцией `getDataFunc`</typeparam>
    /// <returns>Значение из КЭШа</returns>
    public Task<T> GetWithCache<T>(string key,
        Func<Task<T>> getDataFunc,
        TimeSpan? expiration = null,
        CancellationToken token = default);


    /// <summary>
    /// Удаление ключей в КЭШе
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="token">Токен отмены операции</param>
    /// <returns></returns>
    public Task InvalidateCache(string key, CancellationToken token = default);
}