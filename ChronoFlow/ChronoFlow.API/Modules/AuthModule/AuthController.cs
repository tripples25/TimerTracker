using ChronoFlow.API.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace ChronoFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {   
        //List<User> users =  
        [HttpPost("signin")]
        public async Task<IActionResult> SignInAsync([FromBody] SignInRequest signInRequest)
        {
            var user = users.FirstOrDefault(x => x.Email == signInRequest.Email 
            && x.Password == signInRequest.Password); // Добавить хеш на пароль
            // Секрет на Соль можно держать в Config
            if (user is null)
            {
                return BadRequest(new Response(false, "Invalid credentials."));
            }
            var claims = new List<Claim>
            {
                new(type: ClaimTypes.Email, value: signInRequest.Email),
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
            return Ok(new Response(true, "Signed in successfully"));
        }

        [Authorize]
        [HttpGet("signout")]
        public async Task<ActionResult> SignOutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return NoContent();
        }
        
        // Password Change/Recovery ???
    }
}
