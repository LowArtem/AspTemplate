using System.Security.Claims;
using AspAdvancedApp.Core.Dto;
using AspAdvancedApp.Core.Extensions;
using AspAdvancedApp.Core.Model.Auth;
using AspAdvancedApp.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using AutoMapper;
using AspAdvancedApp.Core.Configurations;
using AspAdvancedApp.Core.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace AspAdvancedApp.Data.Services;

public class UserService
{
    private readonly IEfCoreRepository<User> _userRepository;
    private readonly IEfCoreRepository<Role> _roleRepository;
    private readonly IJwtService _jwtService;
    private readonly IMapper _mapper;

    public UserService(IEfCoreRepository<User> userRepository, IEfCoreRepository<Role> roleRepository, IMapper mapper, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
        _jwtService = jwtService;
    }

    /// <summary>
    /// Зарегистрировать нового пользователя
    /// </summary>
    /// <param name="registerDto">данные для регистрации</param>
    /// <returns></returns>
    /// <exception cref="EntityExistsException">если пользователь с таким email уже существует</exception>
    /// <exception cref="ApplicationException">ошибка при создании пользователя</exception>
    public AuthResponseDto RegisterUser(RegisterDto registerDto)
    {
        if (_userRepository.Any(u => u.Email == registerDto.Email))
            throw new EntityExistsException(typeof(User), registerDto.Email);

        var passwordHash = registerDto.Password.Hash();
        var defaultRole = _roleRepository.GetListQuery()
            .AsTracking()
            .FirstOrDefault(r => r.Name == ApplicationConstants.DEFAULT_ROLE_NAME);

        if (defaultRole == null)
        {
            CreateDefaultRole();
            defaultRole = _roleRepository.GetListQuery()
                .AsTracking()
                .FirstOrDefault(r => r.Name == ApplicationConstants.DEFAULT_ROLE_NAME);
        }

        var user = new User
        {
            Email = registerDto.Email,
            PasswordHash = passwordHash,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            MiddleName = registerDto.MiddleName,
            UserRoles = new List<Role> { defaultRole! }
        };

        _userRepository.Add(user);
        _userRepository.SaveChanges();

        var token = GetToken(user.Email, registerDto.Password);
        if (token == null)
        {
            throw new ApplicationException("Error while creating a user");
        }

        return new AuthResponseDto(
            Email: user.Email,
            AccessToken: token,
            Roles: _mapper.Map<List<RoleResponseDto>>(new List<Role> { defaultRole! })
        );
    }

    /// <summary>
    /// Предоставить доступ для зарегистрированного пользователя
    /// </summary>
    /// <param name="loginDto">данные для входа</param>
    /// <returns></returns>
    /// <exception cref="EntityNotFoundException">если пользователя с таким email не существует</exception>
    /// <exception cref="AuthenticationException">неверные данные авторизации</exception>
    public AuthResponseDto LoginUser(LoginDto loginDto)
    {
        var user = _userRepository.GetListQuery()
            .Include(u => u.UserRoles)
            .FirstOrDefault(u => u.Email == loginDto.Email);

        if (user == null)
        {
            throw new EntityNotFoundException(typeof(User), loginDto.Email);
        }

        if (loginDto.Password.Hash() != user.PasswordHash)
        {
            throw new AuthenticationException("Wrong password");
        }

        return new AuthResponseDto(
            Email: user.Email,
            AccessToken: GetToken(loginDto.Email, loginDto.Password)!,
            Roles: user.UserRoles.Select(r => _mapper.Map<RoleResponseDto>(r)).ToList()
        );
    }

    /// <summary>
    /// Добавить роли пользователю
    /// </summary>
    /// <param name="userId">Id пользователя</param>
    /// <param name="roleIds">Id добавляемых ролей</param>
    /// <exception cref="ArgumentException">если не переданы роли</exception>
    /// <exception cref="EntityNotFoundException">если роль или пользователь с переданным Id не существует</exception>
    public void AddRoles(int userId, List<int> roleIds)
    {
        if (roleIds.Count == 0)
        {
            throw new ArgumentException("You have to provide role ids", nameof(roleIds));
        }

        var roles = _roleRepository.GetListQuery()
            .AsTracking()
            .Where(x => roleIds.Contains(x.Id))
            .ToList();

        if (roles.Count() != roleIds.Count)
        {
            throw new EntityNotFoundException(typeof(Role), roleIds);
        }

        var user = _userRepository.GetListQuery()
            .AsTracking()
            .Include(u => u.UserRoles)
            .SingleOrDefault(u => u.Id == userId);

        if (user == null)
        {
            throw new EntityNotFoundException(typeof(User), userId);
        }

        foreach (var r in roles)
        {
            if (r.Users == null)
            {
                r.Users = new List<User>();
            }

            r.Users.Add(user);
        }

        _roleRepository.SaveChanges();
    }

    private void CreateDefaultRole()
    {
        var newRole = new Role
        {
            Name = ApplicationConstants.DEFAULT_ROLE_NAME,
            Description = "Учетная запись обычного пользователя"
        };
        _roleRepository.Add(newRole);
        _roleRepository.SaveChanges();
    }

    private ClaimsIdentity? GetIdentity(string email, string password)
    {
        var passwordHash = password.Hash();

        // Информация о пользователе
        var user = _userRepository.GetListQuery()
            .Include(p => p.UserRoles)
            .FirstOrDefault(x => x.Email == email && x.PasswordHash == passwordHash);

        // Если пользователя нет
        if (user == null)
            return null;

        // Параметры токена
        var claims = new List<Claim>
        {
            new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            new Claim("id", user.Id.ToString())
        };

        // Добавить роли в токен
        const string typeRole = "Role";

        user.UserRoles.Select(p => p.Name)
            .ToList()
            .ForEach(p => claims.Add(new Claim(typeRole, p)));

        ClaimsIdentity claimsIdentity = new(claims, "Token", ClaimsIdentity.DefaultNameClaimType, typeRole);

        return claimsIdentity;
    }


    private string? GetToken(string email, string password)
    {
        var identity = GetIdentity(email, password);

        if (identity == null)
            return null;

        var now = DateTime.UtcNow;

        var jwt = new JwtSecurityToken(
            issuer: _jwtService.Issuer,
            audience: _jwtService.Audience,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(_jwtService.Lifetime)),
            signingCredentials: new SigningCredentials(_jwtService.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }
}