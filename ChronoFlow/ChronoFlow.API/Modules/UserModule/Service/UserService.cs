using System.Reflection;
using System.Security.Claims;
using System.Security.Cryptography;
using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using ChronoFlow.API.Modules.EventsModule;
using ChronoFlow.API.Modules.TemplatesModule;
using ChronoFlow.API.Modules.UserModule.Repository;
using ChronoFlow.API.Modules.UserModule.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Modules.UserModule.Service;

public class UserService : ControllerBase, IUserService
{
    private readonly IUserRepository userRepository;
    private readonly IEventRepository eventRepository;
    private readonly PasswordHasher passwordHasher;

    public UserService(IUserRepository userRepository, PasswordHasher passwordHasher, IEventRepository eventRepository)
    {
        this.userRepository = userRepository;
        this.passwordHasher = passwordHasher;
        this.eventRepository = eventRepository;
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

    public async Task<ActionResult<UserLogInRequest>> Login(UserLogInRequest request)
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
        await HttpContext.SignInAsync(
            // TODO:  вся логика с HttpContext должна жить в контроллере
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

    public async Task<ActionResult> SignOutAsync()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return NoContent();
    }

    public async Task<ActionResult<UserChangePasswordRequest>> ChangePassword(UserChangePasswordRequest request)
    {
        var user = await userRepository.FindAsync(request.Email);
        if (!passwordHasher.VerifyPasswordHash(request.CurrentPassword, user.PasswordHash))
            return BadRequest("Password is incorrect.");

        user.PasswordHash = passwordHasher.CreatePasswordHash(request.NewPassword);
        await userRepository.SaveChangesAsync();

        return NoContent();
    }

    public async Task<ActionResult<IEnumerable<UserEntity>>> GetUsers()
    {
        var users = await userRepository.ToListAsync();

        return Ok(users);
    }

    public async Task<ActionResult<UserEntity>> GetUser(string email)
    {
        var user = await userRepository.FindAsync(email);
        if (user == null)
            return NotFound();

        return Ok(user);
    }

    public async Task<ActionResult<UserEntity>> CreateOrUpdateUser(UserEntity userEntity)
    {
        var user = await userRepository.FindAsync(userEntity.Email);
        if (user == null)
        {
            await userRepository.AddAsync(userEntity);
            return Ok(userEntity);
        }
        var userProp = user.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        var inputUserProp = userEntity.GetType().GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
        for (int i = 0; i < inputUserProp.Length; i++)
        {
            userProp[i].SetValue(userEntity, inputUserProp[i].GetValue(userEntity));
        }
        await userRepository.SaveChangesAsync();

        return Ok(user);
    }

    public async Task<ActionResult> DeleteUser(string email)
    {
        var user = await userRepository.FindAsync(email);
        if (user == null)
            return NotFound();

        userRepository.Remove(user);
        await userRepository.SaveChangesAsync();

        return NoContent();
    }

    public async Task<ActionResult<UserEntity>> AddUserEvent(string email, Guid eventId)
    {
        var user = await userRepository.FindAsync(email);
        var eventEntity = await eventRepository.FindAsync(eventId);
        if (user == null || eventEntity == null)
            return NotFound();

        user.Events.Add(eventEntity);
        await userRepository.SaveChangesAsync();

        return Ok(user);
    }

    public async Task<ActionResult<UserEntity>> DeleteUserEvent(string email, Guid eventId)
    {
        var user = await userRepository.FindAsync(email);
        var eventEntity = await eventRepository.FindAsync(eventId);
        if (user == null || eventEntity == null)
            return NotFound();

        user.Events.Remove(eventEntity);
        await userRepository.SaveChangesAsync();

        return Ok(user);
    }
}