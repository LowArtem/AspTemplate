using AspAdvancedApp.Api.Configurations.Hangfire;
using AspAdvancedApp.Api.Extensions.Application;
using AspAdvancedApp.Api.Mappers;
using AspAdvancedApp.Data.Services;
using AutoMapper;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Prometheus;
using Serilog;

namespace AspAdvancedApp.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddBaseModuleDi("DefaultConnection", Configuration);

        #region Hangfire

        // Add Hangfire services
        services.AddHangfire(configuration => configuration
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UsePostgreSqlStorage(Configuration.GetConnectionString("HangfireConnection"), new PostgreSqlStorageOptions
            {
                DistributedLockTimeout = TimeSpan.FromMinutes(1),
                SchemaName = "hangfire"
            }));

        // Add the processing server as IHostedService
        services.AddHangfireServer();

        #endregion

        // Services can be added here
        services.AddTransient(typeof(UserService), typeof(UserService));


        // Auto Mapper Configurations
        var mapperConfig = new MapperConfiguration(mc => { mc.AddProfile(new MappingProfile()); });

        var mapper = mapperConfig.CreateMapper();
        services.AddSingleton(mapper);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app,
        IApiVersionDescriptionProvider provider,
        IWebHostEnvironment env,
        ILogger<Startup> logger)
    {
        app.MigrateDatabase(logger);

        app.UseBaseServices(env, provider);

        app.UseHangfireDashboard("/api/hangfire", new DashboardOptions
        {
            Authorization = new[] { new AllowAllConnectionsFilter() },
            IgnoreAntiforgeryToken = true
        });

        app.UseSerilogRequestLogging();
        
        app.UseHttpMetrics();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapMetrics();
        });
    }
}