using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.UserModule;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService usersService;

    public UserController(IUserService usersService)
    {
        this.usersService = usersService;
    }

    [HttpPost("register")]
    public Task<ActionResult<UserRegisterRequest>> Register([FromBody] UserRegisterRequest request)
        => usersService.Register(request);


    [HttpPost("login")]
    public Task<ActionResult<UserLogInRequest>> Login([FromBody] UserLogInRequest request)
        => usersService.Login(HttpContext, request);


    [Authorize]
    [HttpGet("signout")]
    public Task<ActionResult> SignOutAsync()
        => usersService.SignOutAsync(HttpContext);


    [Authorize]
    [HttpPost("password")]
    public Task<ActionResult<UserChangePasswordRequest>> ChangePassword([FromBody] UserChangePasswordRequest request)
        => usersService.ChangePassword(request);
}