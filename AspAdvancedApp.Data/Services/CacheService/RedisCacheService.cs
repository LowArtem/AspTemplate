using System.Text.Json;
using System.Text.Json.Serialization;
using AspAdvancedApp.Data.Redis;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace AspAdvancedApp.Data.Services.CacheService;

public class RedisCacheService : ICacheService
{
    private const string PRE_KEY_PROJECT = "asp_application";
    private readonly IDatabase _redisDatabase;
    private readonly ILogger<RedisCacheService> _logger;


    private static readonly JsonSerializerOptions JsonSerializerOptions = new()
    {
        Converters = {new JsonStringEnumConverter()}
    };

    public RedisCacheService(IRedisDatabaseAccessor redisDatabaseAccessor,
        ILogger<RedisCacheService> logger)
    {
        _logger = logger;
        _redisDatabase = redisDatabaseAccessor.GetDatabase();
        redisDatabaseAccessor.GetServer();
    }

    /// <inheritdoc />
    public async Task<string?> GetAsync(string key, CancellationToken token = default)
    {
        try
        {
            return await _redisDatabase.StringGetAsync(FixKey(key)).WaitAsync(token);
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogError(ex, "Не удалось подключиться к Redis");
            return null;
        }
    }

    public async Task<T> GetAsync<T>(string key, CancellationToken token = default)
    {
        var cachedData = await GetAsync(key, token: token);
        return string.IsNullOrWhiteSpace(cachedData)
            ? default
            : JsonSerializer.Deserialize<T>(cachedData, JsonSerializerOptions)!;
    }

    /// <inheritdoc />
    public async Task SetAsync(string key,
        string value,
        TimeSpan? expiration = null,
        CancellationToken token = default)
    {
        try
        {
            await _redisDatabase.StringSetAsync(FixKey(key), value, expiration)
                .WaitAsync(token);
        }
        catch (RedisConnectionException ex)
        {
            _logger.LogError(ex, "Не удалось подключиться к Redis");
        }
    }

    /// <inheritdoc />
    public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken token = default)
    {
        var serializedData = JsonSerializer.Serialize(value);
        await SetAsync(key, serializedData, expiration, token: token);
    }

    /// <inheritdoc />
    public async Task<T> GetWithCache<T>(string key,
        Func<Task<T>> getDataFunc,
        TimeSpan? expiration = null,
        CancellationToken token = default)
    {
        var cachedData = await GetAsync<T>(key, token: token);
        if (cachedData != null)
            return cachedData;

        var data = await getDataFunc().WaitAsync(token);

        if (data != null)
            await SetAsync(key, data, expiration, token: token);

        return data;
    }


    /// <inheritdoc />
    public async Task InvalidateCache(string key, CancellationToken token = default)
    {
        try
        {
            await _redisDatabase.KeyDeleteAsync(key).WaitAsync(token);
        }
        catch (Exception e)
        {
            _logger.LogError(e, @"При удалении ключей ""{CacheKey}"" из Redis произошла ошибка", key);
        }
    }


    private static string FixKey(string key) =>
        !key.StartsWith($"{PRE_KEY_PROJECT}:")
            ? $"{PRE_KEY_PROJECT}:{key}"
            : key;
}