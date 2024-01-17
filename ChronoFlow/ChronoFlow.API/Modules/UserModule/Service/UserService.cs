using System.Security.Claims;
using AutoMapper;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.UserModule.Repository;
using ChronoFlow.API.Modules.UserModule.Requests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.UserModule.Service;

public class UserService : ControllerBase, IUserService
{
    private readonly IMapper mapper;
    private readonly IUserRepository userRepository;
    private readonly IUnifyRepository<EventEntity> eventRepository;
    private readonly PasswordHasher passwordHasher;

    public UserService(
        IMapper mapper,
        IUserRepository userRepository,
        PasswordHasher passwordHasher,
        IUnifyRepository<EventEntity> eventRepository)
    {
        this.mapper = mapper;
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

        throw new Exception("Неправильно. У тя тут HttpContext уже сдох. С ним действия доллжны быть в Контроллере");

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
        throw new Exception("Аналогично");

        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

        return NoContent();
    }

    public async Task<ActionResult<UserChangePasswordRequest>> ChangePassword(UserChangePasswordRequest request)
    {
        var user = await userRepository.FindAsync(request.Email);
        if (user == null)
            return BadRequest("SoSi biby");

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
            await userRepository.AddAsync(userEntity);
        else
            mapper.Map(userEntity, user);

        await userRepository.SaveChangesAsync();
        
        return Ok(user);
    }

    public async Task<ActionResult> DeleteUser(string email)
    {
        var user = await userRepository.FindAsync(email);
        if (user == null)
            return NotFound(); // Best practice - всегда кидать 2xx даже если сущность уже удалена

        userRepository.Remove(user);
        await userRepository.SaveChangesAsync();

        return NoContent();
    }
    
    // Декомпозировать логику. Слишком большая ответственность на сервисе

    /*public async Task<ActionResult<UserEntity>> AddUserEvent(string email, Guid eventId)
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
    }*/

    /*public async Task<ActionResult<AnalyticsResponse>> GetAnalytics(string email, UserAnalyticsRequests request)
    {
        var analyticsEventEntity = new HashSet<EventAnalyticsModule>();
        var user = await userRepository.FindAsync(email);
        var events = user.Events
            .Where(d => d.StartTime >= request.Start && d.EndTime <= request.End)
            .GroupBy(n => n.Template.Name);
        int totalHours = default;
        int totalCount = default;
        foreach (var group in events)
        {
            string name = group.Key;
            int timeInMinutes = default;
            int count = group.Count();
            totalCount += count;
            foreach (var e in group)
            {
                timeInMinutes += (int) (e.EndTime - e.StartTime).Value.TotalMinutes;
                totalHours += timeInMinutes;
            }

            analyticsEventEntity.Add
            (
                new EventAnalyticsModule
                (
                    name,
                    timeInMinutes,
                    timeInMinutes / 60,
                    timeInMinutes * 60,
                    count
                )
            );
        }

        return Ok(new AnalyticsResponse(analyticsEventEntity, totalCount, totalHours));
    }*/
}