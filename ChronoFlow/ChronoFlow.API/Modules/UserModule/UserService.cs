using System.Security.Claims;
using System.Security.Cryptography;
using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Modules.UserModule;

public class UserService : ControllerBase, IUserService
{
    private readonly IUserRepository userRepository;
    private readonly PasswordHasher passwordHasher;

    public UserService(IUserRepository userRepository, PasswordHasher passwordHasher)
    {
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

    public async Task<ActionResult<UserLogInRequest>> Login(HttpContext httpContext, UserLogInRequest request)
    {
        var user = await userRepository.FindAsync(request.Email);
        if (user == null)
            return NotFound();

        // Секрет на Соль можно держать в Config
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
        var user = await userRepository.FirstOrDefaultAsync(request.Email);
        if (!passwordHasher.VerifyPasswordHash(request.CurrentPassword, user.PasswordHash))
            return BadRequest("Password is incorrect.");

        user.PasswordHash = passwordHasher.CreatePasswordHash(request.NewPassword);
        await userRepository.SaveChangesAsync();
        await userRepository.SaveChangesAsync();

        return NoContent();
    }
}