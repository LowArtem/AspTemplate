using AutoMapper;
using AspAdvancedApp.Api.Api._Base;
using AspAdvancedApp.Api.Attributes;
using AspAdvancedApp.Api.Dto.User;
using AspAdvancedApp.Core.Exceptions;
using AspAdvancedApp.Core.Exceptions._Base;
using AspAdvancedApp.Core.Repositories;
using AspAdvancedApp.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace AspAdvancedApp.Api.Api.User;

/// <summary>
/// Контроллер управления пользователями
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[SetRoute]
[Authorize]
public class UserController : BaseCrudController<Core.Model.Auth.User, UserRequestDto, UserResponseDto>
{
    private readonly UserService _service;

    /// <inheritdoc />
    protected override IQueryable<Core.Model.Auth.User> List
        => base.List.Include(u => u.UserRoles);

    /// <inheritdoc />
    public UserController(IEfCoreRepository<Core.Model.Auth.User> repository,
        ILogger<BaseCrudController<Core.Model.Auth.User, UserRequestDto, UserResponseDto>> logger, IMapper mapper,
        UserService service) :
        base(repository, logger, mapper)
    {
        _service = service;
    }

    /// <summary>
    /// Добавление записи
    /// </summary>
    /// <param name="model">Запись</param>
    /// <response code="400">Запись не прошла валидацию</response>
    /// <response code="409">Запись уже существует</response>
    /// <response code="500">При добавлении записи произошла ошибка на сервере</response>   
    /// <returns>Добавленная запись</returns>
    [HttpPost]
    [SwaggerResponse(200, "Запись успешно добавлена. Содержит информацию о добавленной записи", typeof(UserResponseDto))]
    [SwaggerResponse(400, "Ошибка валидации")]
    [SwaggerResponse(409, "Запись уже существует")]
    [SwaggerResponse(500, "Ошибка при добавлении записи")]
    public override ActionResult<UserResponseDto> Add(UserRequestDto model)
    {
        return List.Any(u => u.Email == model.Email)
            ? Conflict("Пользователь с таким Email уже существует")
            : base.Add(model);
    }

    /// <summary>
    /// Добавление записей
    /// </summary>
    /// <param name="models">Записи</param>
    /// <response code="400">Записи не прошли валидацию</response>
    /// <response code="409">Запись уже существует</response>
    /// <response code="500">При добавлении записи произошла ошибка на сервере</response>   
    /// <returns>Добавленная запись</returns>
    [HttpPost("range")]
    [SwaggerResponse(200, "Записи успешно добавлены. Содержит список добавленных записей", typeof(List<UserResponseDto>))]
    [SwaggerResponse(409, "Запись уже существует")]
    [SwaggerResponse(500, "Произошла ошибка при добавлении записей")]
    public override ActionResult<List<UserResponseDto>> AddRange(List<UserRequestDto> models)
    {
        return List.Any(u => models.Select(m => m.Email).Contains(u.Email))
            ? Conflict("Пользователь с таким Email уже существует")
            : base.AddRange(models);
    }

    /// <summary>
    /// Добавить роль пользователю
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <param name="roleId">Id роли</param>
    /// <returns></returns>
    [HttpPost("role")]
    [SwaggerResponse(200, "Роль успешно добавлена")]
    [SwaggerResponse(400, "Неверные данные", typeof(BaseError))]
    [SwaggerResponse(500, "Ошибка при добавлении роли", typeof(BaseError))]
    public IActionResult AddRoleToUser(int userId, int roleId)
    {
        try
        {
            _service.AddRoles(userId, new List<int> { roleId });
            return Ok();
        }
        catch (ArgumentException e)
        {
            return BadRequest(new BaseError("Вы должны передать роли"));
        }
        catch (EntityNotFoundException e)
        {
            return BadRequest(new BaseError("Пользователь или роль с переданным Id не существует"));
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while adding role to user: {e}");
            return StatusCode(500, new BaseError("Ошибка при добавлении роли пользователю"));
        }
    }

    /// <summary>
    /// Добавить роли пользователю
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <param name="rolesId">Id ролей</param>
    /// <returns></returns>
    [HttpPost("roles")]
    [SwaggerResponse(200, "Роли успешно добавлены")]
    [SwaggerResponse(400, "Неверные данные (роли не переданы)", typeof(BaseError))]
    [SwaggerResponse(500, "Ошибка при добавлении роли", typeof(BaseError))]
    public IActionResult AddRoleToUser(int userId, List<int> rolesId)
    {
        try
        {
            _service.AddRoles(userId, rolesId);
            return Ok();
        }
        catch (ArgumentException e)
        {
            return BadRequest(new BaseError("Вы должны передать роли"));
        }
        catch (EntityNotFoundException e)
        {
            return BadRequest(new BaseError("Пользователь или роль с переданным Id не существует"));
        }
        catch (Exception e)
        {
            _logger.LogError($"Error while adding roles to user: {e}");
            return StatusCode(500, new BaseError("Ошибка при добавлении ролей пользователю"));
        }
    }
}