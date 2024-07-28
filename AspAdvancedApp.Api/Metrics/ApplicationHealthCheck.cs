using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace AspAdvancedApp.Api.Metrics;

/// <summary>
/// Проверка доступности и работоспособности приложения
/// </summary>
public class ApplicationHealthCheck : IHealthCheck
{
    private readonly IConfiguration _configuration;
    private readonly string _connectionStringName;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="configuration">конфигурация приложения</param>
    /// <param name="connectionStringName">название строки подключения в секции конфигурации</param>
    public ApplicationHealthCheck(IConfiguration configuration, string connectionStringName)
    {
        _configuration = configuration;
        _connectionStringName = connectionStringName;
    }

    /// <inheritdoc />
    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        var connectionString = _configuration.GetConnectionString(_connectionStringName);
        if (string.IsNullOrEmpty(connectionString))
        {
            return HealthCheckResult.Unhealthy("DB connection string not found");
        }
    
        await using var connection = new NpgsqlConnection(connectionString);
        try
        {
            await connection.OpenAsync(cancellationToken);
            return HealthCheckResult.Healthy();
        }
        catch (Exception e)
        {
            return HealthCheckResult.Unhealthy(e.Message);
        }
    }
}