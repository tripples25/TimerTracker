using System.Security.Claims;
using AutoMapper;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.UserModule.Repository;
using ChronoFlow.API.Modules.UserModule.Requests;
using log4net;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.UserModule.Service;

public class UserService : ControllerBase, IUserService
{
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;
    private readonly PasswordHasher passwordHasher;
    private static readonly ILog log = LogManager.GetLogger(typeof(UserService));

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
            log.Info("The user already exist");
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
        log.Info("The user was successfully added");

        return NoContent();
    }

    public async Task<ActionResult<UserLogInRequest>> Login(UserLogInRequest request, HttpContext httpContext)
    {
        var user = await userRepository.FindAsync(request.Email);

        if (user == null)
        {
            log.Info("The user not found");
            return NotFound();
        }

        if (!passwordHasher.VerifyPasswordHash(request.Password, user.PasswordHash))
        {
            log.Info("Incorrect password");
            return BadRequest("Password is incorrect.");
        }

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

        log.Info("The user was login successfully");
        return Ok(user.Email);
    }

    public async Task<ActionResult> SignOutAsync(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        log.Info("The user was sign out successfully successfully");

        return NoContent();
    }

    public async Task<ActionResult<UserChangePasswordRequest>> ChangePassword(UserChangePasswordRequest request)
    {
        var user = await userRepository.FindAsync(request.Email);
        if (user == null)
        {
            log.Info("User was not detected while changing password");
            return BadRequest("User does not exist");
        }

        if (!passwordHasher.VerifyPasswordHash(request.CurrentPassword, user.PasswordHash))
        {
            log.Info("Password is incorrect");
            return BadRequest("Password is incorrect.");
        }

        user.PasswordHash = passwordHasher.CreatePasswordHash(request.NewPassword);
        await userRepository.SaveChangesAsync();
        log.Info("Password changed");

        return NoContent();
    }

    public async Task<ActionResult<IEnumerable<UserEntity>>> GetUsers()
    {
        var users = await userRepository.ToListAsync();
        log.Info("GET request for users");
        return Ok(users);
    }

    public async Task<ActionResult<UserEntity>> GetUser(string email)
    {
        var user = await userRepository.FindAsync(email);
        if (user == null)
        {
            log.Info("User was not found");
            return NotFound();
        }

        log.Info("GET request for specify user");
        return Ok(user);
    }

    public async Task<ActionResult<UserEntity>> CreateOrUpdateUser(UserEntity userEntity)
    {
        var user = await userRepository.FindAsync(userEntity.Email);

        if (user == null)
        {
            await userRepository.AddAsync(userEntity);
            log.Info("The user was created");
        }

        else
        {
            mapper.Map(userEntity, user);
            log.Info("The user was changed");
        }

        await userRepository.SaveChangesAsync();

        return Ok(user);
    }

    public async Task<ActionResult> DeleteUser(string email)
    {
        var user = await userRepository.FindAsync(email);
        userRepository.Remove(user);

        await userRepository.SaveChangesAsync();
        log.Info("The user was deleted");

        return NoContent();
    }
}