using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ChronoFlow.API.Modules.UserModule;
using System.Security.Cryptography;
using ChronoFlow.API.DAL;
using Microsoft.EntityFrameworkCore;
using ChronoFlow.API.Infra;
using ChronoFlow.API.Models;

namespace ChronoFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UserController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if(context.Users.Any(u => u.Email == request.Email))
            {
                return BadRequest("User already exists.");
            }

            var passwordHash = PasswordHasher.CreatePasswordHash(request.Password, Config.passwordSalt);

            var user = new UserEntity
            {
                Name = request.Name,
                Email =  request.Email,
                PasswordHash = passwordHash,
            };
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return Ok("User successfully created!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogInRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email); 

            if (user is null)
            {
                return BadRequest("User not found.");
            }
            if (!PasswordHasher.VerifyPasswordHash(request.Password, user.PasswordHash, Config.passwordSalt)) // Секрет на Соль можно держать в Config
            {
                return BadRequest("Password is incorrect.");
            }
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
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10), // Потестить что через 10 минут можно всё ещё ходить под
                                                                       // залогиненым пользователем(токен не протух и обновился сам)
                });
            return Ok(user.Id);
        }

        [Authorize]
        [HttpGet("signout")]
        public async Task<ActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }

        [Authorize]
        [HttpPost("password")]
        public async Task<ActionResult> ChangePassword([FromBody] UserChangePasswordRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

            if (!PasswordHasher.VerifyPasswordHash(request.CurrentPassword, user.PasswordHash, Config.passwordSalt))
            {
                return BadRequest("Password is incorrect.");
            }

            user.PasswordHash = PasswordHasher.CreatePasswordHash(request.NewPassword, Config.passwordSalt);
            await context.SaveChangesAsync();
            return Ok("Password successfully changed!");
        }

        // Создать класс который считает хеш, не забыть добаить его в DI

    }
}
