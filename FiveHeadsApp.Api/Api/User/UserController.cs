using AutoMapper;
using FiveHeadsApp.Api.Api._Base;
using FiveHeadsApp.Api.Attributes;
using FiveHeadsApp.Api.Dto.User;
using FiveHeadsApp.Core.Exceptions;
using FiveHeadsApp.Core.Exceptions._Base;
using FiveHeadsApp.Core.Repositories;
using FiveHeadsApp.Data.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace FiveHeadsApp.Api.Api.User;

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
            _service.AddRoles(userId, new List<int> {roleId});
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