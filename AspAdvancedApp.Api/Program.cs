using System.Reflection;
using Serilog;

namespace AspAdvancedApp.Api;

public class Program
{
    /// <summary>
    /// Точка входа
    /// </summary>
    /// <param name="args"></param>
    public static void Main(string[] args)
    {
        // Создание хоста
        CreateHost(args);
    }

    /// <summary>
    /// Создание хоста
    /// </summary>
    /// <param name="args"></param>
    private static void CreateHost(string[] args)
    {
        try
        {
            CreateHostBuilder(args).Build().Run();
        }
        catch (Exception ex)
        {
            Log.Fatal($"Failed to start {Assembly.GetExecutingAssembly().GetName().Name}", ex);
            throw;
        }
    }

    /// <summary>
    /// Конфиг хоста
    /// </summary>
    /// <param name="args"></param>
    /// <returns></returns>
    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            })
            .ConfigureAppConfiguration(configuration =>
            {
                configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                configuration.AddJsonFile(
                    $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json",
                    optional: true);
            })
            .UseSerilog((ctx, lc) => lc
                .WriteTo.Console()
                .ReadFrom.Configuration(ctx.Configuration));
    }
}