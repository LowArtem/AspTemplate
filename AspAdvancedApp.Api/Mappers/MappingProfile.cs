using AspAdvancedApp.Core.Dto;
using AspAdvancedApp.Core.Model.Auth;
using AutoMapper;
using AspAdvancedApp.Api.Dto.Role;
using AspAdvancedApp.Api.Dto.User;
using AspAdvancedApp.Core.Extensions;

namespace AspAdvancedApp.Api.Mappers;

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