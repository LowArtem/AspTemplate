using AutoMapper;
using AspAdvancedApp.Api.Api._Base;
using AspAdvancedApp.Api.Attributes;
using AspAdvancedApp.Api.Dto.Role;
using AspAdvancedApp.Core.Dto;
using AspAdvancedApp.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AspAdvancedApp.Api.Api.Role;

/// <summary>
/// Контроллер управления ролями
/// </summary>
[ApiController]
[ApiVersion("1.0")]
[SetRoute]
[Authorize]
public class RoleController : BaseCrudController<Core.Model.Auth.Role, RoleRequestDto, RoleResponseDto>
{
    public RoleController(IEfCoreRepository<Core.Model.Auth.Role> repository,
        ILogger<BaseCrudController<Core.Model.Auth.Role, RoleRequestDto, RoleResponseDto>> logger, IMapper mapper) :
        base(repository, logger, mapper)
    {
    }
}