using System.Security.Claims;
using ChronoFlow.API.DAL;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Modules.UserModule;

public interface IUsersService
{
    Task<ActionResult<Guid>> Login(UserLogInRequest request);
}

public class UsersService : ControllerBase, IUsersService
{
    private readonly IUsersRepository usersRepository;
    private readonly PasswordHasher passwordHasher;

    public UsersService(
        IUsersRepository usersRepository,
        PasswordHasher passwordHasher)
    {
        this.usersRepository = usersRepository;
        this.passwordHasher = passwordHasher;
    }

    public async Task<ActionResult<Guid>> Login(UserLogInRequest request)
    {
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

        return Ok(user.Id);
    }
}