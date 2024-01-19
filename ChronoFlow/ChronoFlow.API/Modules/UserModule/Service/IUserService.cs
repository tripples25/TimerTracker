using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.UserModule.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.UserModule.Service;

public interface IUserService
{
    public Task<ActionResult<UserRegisterRequest>> Register(UserRegisterRequest request);
    Task<ActionResult<UserLogInRequest>> Login(UserLogInRequest request, HttpContext httpContext);
    public Task<ActionResult> SignOutAsync(HttpContext httpContext);
    public Task<ActionResult<UserChangePasswordRequest>> ChangePassword([FromBody] UserChangePasswordRequest request);
    Task<ActionResult<IEnumerable<UserEntity>>> GetUsers();
    Task<ActionResult<UserEntity>> GetUser(string email);
    Task<ActionResult<UserEntity>> CreateOrUpdateUser(UserEntity userEntity);
    Task<ActionResult> DeleteUser(string Email);


}