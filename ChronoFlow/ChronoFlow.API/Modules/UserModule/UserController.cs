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
    public Task<IActionResult> Register([FromBody] UserRegisterRequest request)
        => usersService.Register(request);


    [HttpPost("login")]
    public Task<ActionResult<Guid>> Login([FromBody] UserLogInRequest request)
        => usersService.Login(request);


    [Authorize]
    [HttpGet("signout")]
    public Task<ActionResult> SignOutAsync()
        => usersService.SignOutAsync();


    [Authorize]
    [HttpPost("password")]
    public Task<ActionResult> ChangePassword([FromBody] UserChangePasswordRequest request)
        => usersService.ChangePassword(request);
}