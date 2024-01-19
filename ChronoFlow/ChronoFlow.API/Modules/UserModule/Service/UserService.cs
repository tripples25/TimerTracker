using System.Security.Claims;
using AutoMapper;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.UserModule.Repository;
using ChronoFlow.API.Modules.UserModule.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.UserModule.Service;

public class UserService : ControllerBase, IUserService
{
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;
    private readonly PasswordHasher passwordHasher;

    public UserService(
        IMapper mapper,
        IUserRepository userRepository,
        PasswordHasher passwordHasher
        )
    {
        this.mapper = mapper;
        this.userRepository = userRepository;
        this.passwordHasher = passwordHasher;
    }

    public async Task<ActionResult<UserRegisterRequest>> Register(UserRegisterRequest request)
    {
        if (userRepository.Any(request.Email))
        {
            return BadRequest("User already exists.");
        }

        var passwordHash = passwordHasher.CreatePasswordHash(request.Password);

        var user = new UserEntity
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHash,
        };

        await userRepository.AddAsync(user);
        await userRepository.SaveChangesAsync();

        return NoContent();
    }

    public async Task<ActionResult<UserLogInRequest>> Login(UserLogInRequest request, HttpContext httpContext)
    {
        var user = await userRepository.FindAsync(request.Email);
        if (user == null)
            return NotFound();

        if (!passwordHasher.VerifyPasswordHash(request.Password, user.PasswordHash))
            return BadRequest("Password is incorrect.");

        var claims = new List<Claim>
        {
            new(type: ClaimTypes.Email, value: request.Email),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

        await httpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1),
            });

        return Ok(user.Email);
    }

    public async Task<ActionResult> SignOutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return NoContent();
    }

    public async Task<ActionResult<UserChangePasswordRequest>> ChangePassword(UserChangePasswordRequest request)
    {
        var user = await userRepository.FindAsync(request.Email);
        if (user == null)
            return BadRequest("SoSi biby");

        if (!passwordHasher.VerifyPasswordHash(request.CurrentPassword, user.PasswordHash))
            return BadRequest("Password is incorrect.");

        user.PasswordHash = passwordHasher.CreatePasswordHash(request.NewPassword);
        await userRepository.SaveChangesAsync();

        return NoContent();
    }

    public async Task<ActionResult<IEnumerable<UserEntity>>> GetUsers()
    {
        var users = await userRepository.ToListAsync();

        return Ok(users);
    }

    public async Task<ActionResult<UserEntity>> GetUser(string email)
    {
        var user = await userRepository.FindAsync(email);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    public async Task<ActionResult<UserEntity>> CreateOrUpdateUser(UserEntity userEntity)
    {
        var user = await userRepository.FindAsync(userEntity.Email);
        if (user == null)
            await userRepository.AddAsync(userEntity);
        else
            mapper.Map(userEntity, user);

        await userRepository.SaveChangesAsync();
        
        return Ok(user);
    }

    public async Task<ActionResult> DeleteUser(string email)
    {
        var user = await userRepository.FindAsync(email);

        userRepository.Remove(user);
        await userRepository.SaveChangesAsync();

        return NoContent();
    }
}