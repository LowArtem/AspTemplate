using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace FiveHeadsApp.Api.Attributes;

/// <summary>
/// Реализация атрибута для задания маршрута до методов контроллера
/// </summary>
public class SetRouteAttribute : RouteAttribute
{
    /// <inheritdoc />
    public SetRouteAttribute() : base(GetTemplate("v{version:ApiVersion}/[controller]"))
    {
    }

    /// <inheritdoc />
    public SetRouteAttribute(string name) : base(GetTemplate("v{version:ApiVersion}/" + name))
    {
    }

    /// <summary>
    /// Получение полного пути к методу API
    /// </summary>
    /// <param name="template"></param>
    /// <returns></returns>
    private static string GetTemplate(string template)
    {
        var templateReplace = new Regex(@"^/?api/").Replace(template, "");

        return $"/api/{Assembly.GetEntryAssembly()?.GetName()?.Name?.ToLower().Replace(".", "-")}/{templateReplace}";
    }
}