using ChronoFlow.API.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.Modules.UserModule;

public interface IUserRepository
{
    bool Any(string email);
    Task<UserEntity?> FindAsync(string email);
    public Task<UserEntity?> FirstOrDefaultAsync(string email);
    public Task<int> SaveChangesAsync();
    public Task<EntityEntry<UserEntity>> AddAsync(UserEntity email);
}