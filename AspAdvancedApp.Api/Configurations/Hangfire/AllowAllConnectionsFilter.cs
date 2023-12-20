using Hangfire.Dashboard;

namespace AspAdvancedApp.Api.Configurations.Hangfire;

/// <summary>
/// Allow all connections to Hangfire
/// </summary>
public class AllowAllConnectionsFilter : IDashboardAuthorizationFilter
{
    /// <inheritdoc />
    public bool Authorize(DashboardContext context)
    {
        // Allow outside

        return true;
    }
}