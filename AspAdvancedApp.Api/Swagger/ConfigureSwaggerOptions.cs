using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AspAdvancedApp.Api.Swagger;

/// <summary>
/// Настраивает параметры генерации Swagger.
/// </summary>
/// <remarks>Это позволяет при управлении версиями API определять документ Swagger для каждой версии API после
/// <see cref="IApiVersionDescriptionProvider"/> служба была разрешена из контейнера службы.</remarks>
public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ConfigureSwaggerOptions"/> class.
    /// </summary>
    /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> используемый для создания документов Swagger.</param>
    public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => this._provider = provider;

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        // добавляем документ swagger для каждой обнаруженной версии API
        // примечание: вы можете пропустить или задокументировать устаревшие версии API по-другому
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }
    }

    private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var info = new OpenApiInfo()
        {
            Title = $"{configuration["Swagger:Title"]} API {description.ApiVersion}",
            Version = description.ApiVersion.ToString(),
            Description = $"{configuration["Swagger:Description"]}",
            License = new OpenApiLicense() { Name = "MIT", Url = new Uri("https://opensource.org/licenses/MIT") }
        };

        if (description.IsDeprecated)
        {
            info.Description += " This API version has been deprecated.";
        }

        return info;
    }
}