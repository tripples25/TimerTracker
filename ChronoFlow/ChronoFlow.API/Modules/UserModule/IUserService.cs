using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.UserModule;

public interface IUserService
{
    public Task<IActionResult> Register(UserRegisterRequest request);
    Task<ActionResult<UserLogInRequest>> Login(UserLogInRequest request);
    public Task<ActionResult> SignOutAsync();
    public Task<ActionResult> ChangePassword([FromBody] UserChangePasswordRequest request);
}