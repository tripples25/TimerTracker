using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.UserModule.Requests;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.UserModule.Service;

public interface IUserService
{
    public Task<ActionResult<UserRegisterRequest>> Register(UserRegisterRequest request);
    Task<ActionResult<UserLogInRequest>> Login(UserLogInRequest request);
    public Task<ActionResult> SignOutAsync();
    public Task<ActionResult<UserChangePasswordRequest>> ChangePassword([FromBody] UserChangePasswordRequest request);
    Task<ActionResult<IEnumerable<UserEntity>>> GetUsers();
    Task<ActionResult<UserEntity>> GetUser(string Email);
    Task<ActionResult<UserEntity>> CreateOrUpdateUser(UserEntity userEntity);
    Task<ActionResult> DeleteUser(string Email);
    Task<ActionResult<UserEntity>> AddUserEvent(string email, Guid eventId);
    public Task<ActionResult<UserEntity>> DeleteUserEvent(string email, Guid eventId);

}