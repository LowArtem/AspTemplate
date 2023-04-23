using FiveHeadsApp.Core.Dto;
using FiveHeadsApp.Core.Model.Auth;
using AutoMapper;
using FiveHeadsApp.Api.Dto.Role;
using FiveHeadsApp.Api.Dto.User;
using FiveHeadsApp.Core.Extensions;

namespace FiveHeadsApp.Api.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, AuthResponseDto>()
            .ForMember(
                dest => dest.Roles,
                opt =>
                    opt.MapFrom(u => u.UserRoles.Select(r => r.Id)));

        CreateMap<UserRequestDto, User>()
            .ForMember(dest => dest.PasswordHash,
                opt => opt.MapFrom(x => x.Password.Hash()));
        
        CreateMap<User, UserResponseDto>();
        CreateMap<Role, RoleResponseDto>();
        CreateMap<RoleRequestDto, Role>();
    }
}