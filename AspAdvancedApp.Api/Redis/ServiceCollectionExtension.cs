using AspAdvancedApp.Data.Redis;
using AspAdvancedApp.Data.Services.CacheService;

namespace AspAdvancedApp.Api.Redis;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddRedis(this IServiceCollection services, IConfiguration configuration)
    {
        const string defaultConnection = "redis:6379,abortConnect=false";
        var connection = configuration.GetValue<string>("Redis:ConnectionString");

        if (string.IsNullOrWhiteSpace(connection))
            connection = defaultConnection;


        services.AddScoped<IRedisDatabaseAccessor>(_ => new RedisDatabaseAccessor(connection));
        services.AddScoped<ICacheService, RedisCacheService>();
        return services;
    }
}