using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.UserModule.Requests;
using ChronoFlow.API.Modules.UserModule.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.UserModule;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IUserService usersService;

    public UsersController(IUserService usersService)
    {
        this.usersService = usersService;
    }

    [HttpPost("register")]
    public Task<ActionResult<UserRegisterRequest>> Register([FromBody] UserRegisterRequest request)
        => usersService.Register(request);

    [HttpPost("login")]
    public Task<ActionResult<UserLogInRequest>> Login([FromBody] UserLogInRequest request)
        => usersService.Login(request, this.HttpContext);

    [Authorize]
    [HttpGet("signout")]
    public Task<ActionResult> SignOutAsync()
        => usersService.SignOutAsync(this.HttpContext);

    [Authorize]
    [HttpPost("password")]
    public Task<ActionResult<UserChangePasswordRequest>> ChangePassword([FromBody] UserChangePasswordRequest request)
        => usersService.ChangePassword(request);

    [HttpGet]
    public Task<ActionResult<IEnumerable<UserEntity>>> GetUsers()
        => usersService.GetUsers();

    [HttpGet("{email}")]
    public Task<ActionResult<UserEntity>> GetUser([FromRoute] string email)
        => usersService.GetUser(email);

    [HttpDelete("{email}")]
    public Task<ActionResult> DeleteUser([FromRoute] string email)
        => usersService.DeleteUser(email);

    [HttpPost]
    public Task<ActionResult<UserEntity>> CreateOrUpdateUser([FromBody] UserEntity userEntity)
        => usersService.CreateOrUpdateUser(userEntity);
}