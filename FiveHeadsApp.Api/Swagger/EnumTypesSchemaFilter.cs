﻿using System.Xml.Linq;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FiveHeadsApp.Api.Swagger;

/// <summary>
/// Swashbuckle Schema Filter
/// </summary>
public class EnumTypesSchemaFilter : ISchemaFilter
{
    private readonly XDocument? _xmlComments = null;

    /// <inheritdoc />
    public EnumTypesSchemaFilter(string xmlPath)
    {
        if (File.Exists(xmlPath))
        {
            _xmlComments = XDocument.Load(xmlPath);
        }
    }

    /// <inheritdoc />
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (_xmlComments == null) return;

        if (schema.Enum != null && schema.Enum.Count > 0 &&
            context.Type != null && context.Type.IsEnum)
        {
            schema.Description += "<p>Members:</p><ul>";

            var fullTypeName = context.Type.FullName;

            foreach (var enumMemberName in schema.Enum.OfType<OpenApiString>().Select(v => v.Value))
            {
                var fullEnumMemberName = $"F:{fullTypeName}.{enumMemberName}";

                var enumMemberComments = _xmlComments.Descendants("member")
                    .FirstOrDefault(m => m.Attribute("name")!.Value.Equals(fullEnumMemberName, StringComparison.OrdinalIgnoreCase));
                if (enumMemberComments == null) continue;

                var summary = enumMemberComments.Descendants("summary").FirstOrDefault();
                if (summary == null) continue;

                schema.Description += $"<li><i>{enumMemberName}</i> - {summary.Value.Trim()}</li>";
            }

            schema.Description += "</ul>";
        }
    }
}