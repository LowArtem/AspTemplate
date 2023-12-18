using StackExchange.Redis;

namespace AspAdvancedApp.Data.Redis;

public interface IRedisDatabaseAccessor
{
    IDatabase GetDatabase(int dbNumber = -1);
    IServer GetServer();
}