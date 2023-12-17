using System.Runtime.ExceptionServices;
using AspAdvancedApp.Core.Extensions;

namespace AspAdvancedApp.Api.Middlewares;

/// <summary>
/// Логирование http запросов
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;

    /// <summary>
    /// Конструкторы
    /// </summary>
    /// <param name="next"></param>
    /// <param name="logger"></param>
    public RequestLoggingMiddleware(RequestDelegate next, ILoggerFactory logger)
    {
        _next = next;
        _logger = logger.CreateLogger<RequestLoggingMiddleware>();
    }

    /// <summary>
    /// Запрос
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task InvokeAsync(HttpContext context)
    {
        var req = GetInfoAboutRequest(context);
        _logger.LogInformation(req);

        try
        {
            await this._next(context);
        }
        catch (Exception ex)
        {
            // Произошло исключение на этапе выполнения запроса
            _logger.LogError($"{req}\n{GetInformationError(context, ex)}");
            ExceptionDispatchInfo.Capture(ex).Throw();
        }

        // Если не было исключения и запрос отработал нормально
        if (context.Response.StatusCode == 200)
        {
            _logger.LogInformation($"{req}\nЗапрос успешно завершен");
        }
        else
        {
            _logger.LogError($"{req}\n{GetInformationError(context)}");
        }
    }

    /// <summary>
    /// Ошибка получения информации
    /// </summary>
    /// <param name="context"></param>
    /// <param name="ex"></param>
    /// <returns></returns>
    private string GetInformationError(HttpContext context, Exception ex = null)
    {
        var req = GetInfoAboutRequest(context);
        _logger.LogInformation(req);

        if (ex == null)
        {
            return $"{req}\nЗапрос завершился с [{context.Response.StatusCode}] кодом ошибки.";
        }
        else
        {
            return $"{req}\nЗапрос завершился с исключением. Информация об ошибке:\n{ex}";
        }
    }

    /// <summary>
    /// Вывод информации о запросе
    /// </summary>
    /// <param name="httpContext">Данные запроса</param>
    /// <returns>Информация о запросе. Содержит: метод, Path, параметры запроса</returns>
    public static string GetInfoAboutRequest(HttpContext httpContext)
    {
        var req = httpContext.Request;

        var _params = req.Query.Select(p =>
        {
            return $"   {p.Key} = \"{p.Value}\"";
        }).Join('\n');

        return $"[{req.Method}] {req.Path}\n{_params}";
    }
}