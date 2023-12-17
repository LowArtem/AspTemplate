using System.Reflection;
using AspAdvancedApp.Api.Middlewares;
using AspAdvancedApp.Data;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace AspAdvancedApp.Api.Extensions.Application;

/// <summary>
/// ApplicationBuilder
/// </summary>
public static class ApplicationBuilderExtensions
{
    /// <summary>
    /// Использование базовых сервисов 
    /// </summary>
    /// <param name="app"></param>
    /// <param name="provider"></param>
    /// <param name="env"></param>
    public static void UseBaseServices(this IApplicationBuilder app, IWebHostEnvironment env,
        IApiVersionDescriptionProvider provider)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        app.UseJwtAuthentication();

        #region Middleware

        app.UseMiddleware<RequestLoggingMiddleware>();

        #endregion

        app.UseSwaggerService(provider);

        app.UseCors(builder =>
        {
            builder.AllowAnyOrigin();
            builder.AllowAnyMethod();
            builder.AllowAnyHeader();
        });

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller:slugify}/{action:slugify}/{id?}");
        });
    }

    /// <summary>
    /// Применение миграций БД
    /// </summary>
    /// <param name="app"></param>
    public static void MigrateDatabase(this IApplicationBuilder app,
        ILogger logger)
    {
        logger.LogInformation("Начало миграций");

        try
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
            {
                serviceScope.ServiceProvider.GetRequiredService<ApplicationContext>().Database.Migrate();
            }

            logger.LogInformation("Миграции успешно завершены");
        }
        catch (Exception ex)
        {
            logger.LogError($"Во время миграций произошла ошибка: {ex}");
        }
    }

    /// <summary>
    /// Внедрнение автогенерируемой документации API - Swagger
    /// </summary>
    /// <param name="app"></param>
    /// <param name="provider"></param>
    private static void UseSwaggerService(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        var isDevelopment = app.ApplicationServices.GetService<IWebHostEnvironment>()!.IsDevelopment();

        var assemblyName = Assembly.GetEntryAssembly()!.GetName().GetNameDashCase();
        var prefixApi = isDevelopment ? "" : $"/api/{assemblyName}";

        app.UseSwagger();

        app.UseSwaggerUI(
            options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        $"{prefixApi}/swagger/{description.GroupName}/swagger.json",
                        description.GroupName.ToUpperInvariant());
                }

                options.DocExpansion(DocExpansion.None);
            });
    }

    /// <summary>
    /// Использование JWT токенов
    /// </summary>
    /// <param name="app"></param>
    private static void UseJwtAuthentication(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}