using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ChronoFlow.API.Modules.UserModule;
using System.Security.Cryptography;

namespace ChronoFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        //List<User> users =  
        private readonly DataContext context;

        public UserController(DataContext context)
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
            context.Users.Add(user);
            await context.SaveChangesAsync();
            return Ok("User successfully created!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLogInRequest request)
        {
            var user = await context.Users.FirstOrDefault(u => u.Email == request.Email); 
   
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
                new(type: ClaimTypes.Name,value: user.Name)
            };
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    AllowRefresh = true,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
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
        [HttpPost("change-password")]
        public async Task<ActionResult> ChangePassword([FromBody] UserChangePasswordRequest request)
        {
            var user = await context.Users.FirstOrDefault(u => u.Email == request.Email);

            if (!VerifyPasswordHash(request.CurrentPassword, user.PasswordHash, /*passwordSalt*/))
            {
                return BadRequest("Password is incorrect.");
            }

            CreatePasswordHash(request.NewPassword,
            out byte[] passwordHash,
            out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            await context.SaveChangesAsync();
            return Ok("Password successfully changed!");
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512(passwordSalt);
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
}
