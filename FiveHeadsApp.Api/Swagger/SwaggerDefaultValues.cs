using System.Text.Json;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace FiveHeadsApp.Api.Swagger;

/// <summary>
/// Представляет фильтр операции Swagger/Swashbuckle, используемый для документирования неявного параметра версии API.
/// </summary>
/// <remarks>Этот <see cref="IOperationFilter"/> требуется только из-за ошибок в <see cref="SwaggerGenerator"/>.
/// Как только они будут исправлены и опубликованы, этот класс можно будет удалить</remarks>
public class SwaggerDefaultValues : IOperationFilter
{
    /// <summary>
    /// Применяет фильтр к указанной операции с использованием заданного контекста.
    /// </summary>
    /// <param name="operation">Операция, к которой применяется фильтр.</param>
    /// <param name="context">Текущий контекст фильтра операции.</param>
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        var apiDescription = context.ApiDescription;

        operation.Deprecated |= apiDescription.IsDeprecated();

        foreach (var responseType in context.ApiDescription.SupportedResponseTypes)
        {
            var responseKey = responseType.IsDefaultResponse ? "default" : responseType.StatusCode.ToString();
            var response = operation.Responses[responseKey];

            foreach (var contentType in response.Content.Keys)
            {
                if (responseType.ApiResponseFormats.All(x => x.MediaType != contentType))
                {
                    response.Content.Remove(contentType);
                }
            }
        }

        if (operation.Parameters == null)
        {
            return;
        }

        foreach (var parameter in operation.Parameters)
        {
            var description = apiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);

            parameter.Description ??= description.ModelMetadata?.Description;

            if (parameter.Schema.Default == null && description.DefaultValue != null)
            {
                var json = JsonSerializer.Serialize(description.DefaultValue, description.ModelMetadata!.ModelType);
                parameter.Schema.Default = OpenApiAnyFactory.CreateFromJson(json);
            }

            parameter.Required |= description.IsRequired;
        }
    }
}