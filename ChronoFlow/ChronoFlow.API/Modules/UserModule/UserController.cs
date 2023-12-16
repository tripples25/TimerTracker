using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ChronoFlow.API.DAL;
using Microsoft.EntityFrameworkCore;
using ChronoFlow.API.DAL.Entities;

namespace ChronoFlow.API.Modules.UserModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly PasswordHasher passwordHasher;

        public UserController(ApplicationDbContext context, PasswordHasher passwordHashers)
        {
            this.context = context;
            passwordHasher = passwordHashers;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        {
            if (context.Users.Any(u => u.Email == request.Email))
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
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogInRequest request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);

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
            Console.WriteLine(user);
            if (!passwordHasher.VerifyPasswordHash(request.CurrentPassword, user.PasswordHash))
                return BadRequest("Password is incorrect.");

            user.PasswordHash = passwordHasher.CreatePasswordHash(request.NewPassword);
            await context.SaveChangesAsync();

            return Ok("Password successfully changed!");
        }
    }
}
