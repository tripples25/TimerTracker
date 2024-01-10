using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.UserModule;

public interface IUserService
{
    public Task<ActionResult<UserRegisterRequest>> Register(UserRegisterRequest request);
    Task<ActionResult<UserLogInRequest>> Login(HttpContext httpContext, UserLogInRequest request);
    public Task<ActionResult> SignOutAsync(HttpContext httpContext);
    public Task<ActionResult<UserChangePasswordRequest>> ChangePassword([FromBody] UserChangePasswordRequest request);
}