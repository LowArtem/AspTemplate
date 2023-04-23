using FiveHeadsApp.Api.Attributes;
using FiveHeadsApp.Core.Dto;
using FiveHeadsApp.Core.Exceptions;
using FiveHeadsApp.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace FiveHeadsApp.Api.Api.Auth;

/// <summary>
/// Авторизация
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[SetRoute]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly UserService _service;
    private readonly ILogger<AuthController> _logger;

    public AuthController(UserService service, ILogger<AuthController> logger)
    {
        _service = service;
        _logger = logger;
    }

    /// <summary>
    /// Пинг
    /// </summary>
    /// <returns></returns>
    [HttpGet("hello")]
    [AllowAnonymous]
    public IActionResult Ping() => Ok("Hello");

    /// <summary>
    /// Пинг с авторизацией
    /// </summary>
    /// <returns></returns>
    [HttpGet("hello-auth")]
    [SwaggerResponse(200, "Hello ответ", typeof(string))]
    [SwaggerResponse(401, "Ошибка авторизации")]
    [SwaggerResponse(403, "Нет доступа")]
    public IActionResult PingAuth() =>
        Ok($"Hello, {User.Identity!.Name}");

    /// <summary>
    /// Зарегистрировать нового пользователя
    /// </summary>
    /// <param name="registerDto">данные регистрации</param>
    /// <returns>логин и токен пользователя</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    [SwaggerResponse(200, "Пользователь успешно зарегистрирован", typeof(AuthResponseDto))]
    [SwaggerResponse(400, "Неверный формат данных")]
    [SwaggerResponse(409, "Пользователь с таким email уже существует", typeof(string))]
    [SwaggerResponse(500, "Ошибка при регистрации пользователя", typeof(string))]
    public IActionResult Register(RegisterDto registerDto)
    {
        try
        {
            return Ok(_service.RegisterUser(registerDto));
        }
        catch (EntityExistsException)
        {
            return Conflict("Пользователь с таким email уже существует");
        }
        catch (Exception e)
        {
            _logger.LogError($"Произошла ошибка при регистрации пользователя: {e}");
            return StatusCode(500, $"Произошла ошибка при регистрации пользователя: {e}");
        }
    }

    /// <summary>
    /// Вход для зарегистрированного пользователя
    /// </summary>
    /// <param name="loginDto">данные для входа</param>
    /// <returns>логин и токен пользователя</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    [SwaggerResponse(200, "Пользователь успешно залогинен", typeof(AuthResponseDto))]
    [SwaggerResponse(400, "Неверный формат данных")]
    [SwaggerResponse(404, "Пользователь с таким email не существует", typeof(string))]
    [SwaggerResponse(500, "Ошибка при логине пользователя", typeof(string))]
    public IActionResult Login(LoginDto loginDto)
    {
        try
        {
            return Ok(_service.LoginUser(loginDto));
        }
        catch (EntityNotFoundException)
        {
            return NotFound("Пользователь с таким email не существует");
        }
        catch (Exception e)
        {
            _logger.LogError($"Произошла ошибка при получении пользователя: {e}");
            return StatusCode(500, $"Произошла ошибка при получении пользователя: {e}");
        }
    }
}