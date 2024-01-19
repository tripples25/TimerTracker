using ChronoFlow.API.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.Modules.UserModule.Repository;

public interface IUserRepository
{
    bool Any(string email);
    Task<UserEntity?> FindAsync(string email);
    public Task<int> SaveChangesAsync();
    public Task<EntityEntry<UserEntity>> AddAsync(UserEntity email);
    public Task<List<UserEntity>> ToListAsync();
    public void Remove(UserEntity userEntity);
    public EntityEntry<UserEntity> Update(UserEntity userEntity);

}