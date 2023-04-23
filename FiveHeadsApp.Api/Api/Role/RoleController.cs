﻿using AutoMapper;
using FiveHeadsApp.Api.Api._Base;
using FiveHeadsApp.Api.Attributes;
using FiveHeadsApp.Api.Dto.Role;
using FiveHeadsApp.Core.Dto;
using FiveHeadsApp.Core.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FiveHeadsApp.Api.Api.Role;

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