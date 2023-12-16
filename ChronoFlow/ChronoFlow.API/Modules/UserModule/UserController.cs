using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ChronoFlow.API.DAL;
using Microsoft.EntityFrameworkCore;
using ChronoFlow.API.DAL.Entities;
using System.Security.Cryptography;

namespace ChronoFlow.API.Modules.UserModule
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly PasswordHasher passwordHasher;
        private readonly IUsersService usersService;

        public UserController(IUsersService usersService, ApplicationDbContext context, PasswordHasher passwordHasher)
        {
            this.usersService = usersService;
            this.context = context;
            this.passwordHasher = passwordHasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterRequest request)
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

        [HttpPost("login")]
        public Task<ActionResult<Guid>> Login([FromBody] UserLogInRequest request)
            => usersService.Login(request);

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
            if (!passwordHasher.VerifyPasswordHash(request.CurrentPassword, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Password is incorrect.");

            user.PasswordHash = passwordHasher.CreatePasswordHash(request.NewPassword, user.PasswordSalt);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
