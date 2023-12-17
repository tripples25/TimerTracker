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
    private readonly ApplicationDbContext context;
    private readonly IUsersRepository usersRepository;
    private readonly PasswordHasher passwordHasher;

    public UserService(IUsersRepository usersRepository,ApplicationDbContext context, PasswordHasher passwordHasher)
    {
        this.usersRepository = usersRepository;
        this.context = context;
        this.passwordHasher = passwordHasher;
    }

    public async Task<IActionResult> Register(UserRegisterRequest request)
    {
        if (context.Users.Any(u => u.Email == request.Email))
        {
            return BadRequest("User already exists.");
        }
        var passwordSalt = new HMACSHA512().Key; // Завязаться на Соль из Config
        var passwordHash = passwordHasher.CreatePasswordHash(request.Password, passwordSalt);

        var user = new UserEntity
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };
        
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();

        return NoContent();
    }

    public async Task<ActionResult<Guid>> Login(UserLogInRequest request)
    {
        //var user = await usersRepository.FindAsync(request.Email);
        var user = await usersRepository.FindAsync(request.Email);
        if (user == null)
            return NotFound();

        // Секрет на Соль можно держать в Config
        if (!passwordHasher.VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Password is incorrect.");

        var claims = new List<Claim>
        {
            new(type: ClaimTypes.Email, value: request.Email),
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        await HttpContext.SignInAsync(
            CookieAuthenticationDefaults.AuthenticationScheme,
            new ClaimsPrincipal(identity),
            new AuthenticationProperties
            {
                IsPersistent = true,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(100), // Потестить что через 10 минут можно всё ещё ходить под
                // залогиненым пользователем(токен не протух и обновился сам)
            });

        //return Ok(user.Id);
        return Ok(user.Email);
    }

    public async Task<ActionResult> SignOutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return NoContent();
    }

    public async Task<ActionResult> ChangePassword(UserChangePasswordRequest request)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
        if (!passwordHasher.VerifyPasswordHash(request.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            return BadRequest("Password is incorrect.");

        user.PasswordHash = passwordHasher.CreatePasswordHash(request.NewPassword, user.PasswordSalt);
        await context.SaveChangesAsync();

        return NoContent();
    }
}