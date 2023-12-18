using StackExchange.Redis;

namespace AspAdvancedApp.Data.Redis;

public class RedisDatabaseAccessor : IRedisDatabaseAccessor
{
    private readonly IConnectionMultiplexer _connectionMultiplexer;

    public RedisDatabaseAccessor(string connectionString)
    {
        _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
    }

    public IDatabase GetDatabase(int dbNumber = -1) => _connectionMultiplexer.GetDatabase(dbNumber);

    public IServer GetServer()
    {
        var endpoint = _connectionMultiplexer.GetEndPoints();
        return _connectionMultiplexer.GetServer(endpoint.First());
    }

    public void Dispose()
    {
        GC.SuppressFinalize(this);
        _connectionMultiplexer.Dispose();
    }
}