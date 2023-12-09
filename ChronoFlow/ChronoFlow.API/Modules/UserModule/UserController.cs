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

            CreatePasswordHash(request.Password,
                out byte[] passwordHash,
                out byte[] passwordSalt);

            var user = new User
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
            if (!VerifyPasswordHash(request.Password, user.PasswordHash, /*passwordSalt*/)) // Секрет на Соль можно держать в Config
            {
                return BadRequest("Password is incorrect.");
            }
            var claims = new List<Claim>
            {
                new(type: ClaimTypes.Email, value: request.Email),
                new(type: ClaimTypes.Name,value: user.Name) //  Нет нужды хранить два идентификатора, тк у ниходна и та же цель(лишняя инфа)
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

            if (!VerifyPasswordHash(request.CurrentPassword, user.PasswordHash))
            {
                return BadRequest("Password is incorrect.");
            }

            CreatePasswordHash(request.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            await context.SaveChangesAsync();
            return Ok("Password successfully changed!");
        }

        // out - bad prectice. Лучше использовать хотя бы кортежи(возвращать значение лучше чем out)
        // Создать класс который считает хеш, не забыть добаить его в DI
        private (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(
            string password, 
            byte[] passwordHash,
            byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, string passwordHash)
        {
            using var hmac = new HMACSHA512();
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
