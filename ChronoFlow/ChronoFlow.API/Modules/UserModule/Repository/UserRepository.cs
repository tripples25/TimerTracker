using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.Modules.UserModule.Repository;


public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext context;

    public UserRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public bool Any(string email)
        => context.Users.Any(u => u.Email == email);

    public async Task<UserEntity?> FindAsync(string email)
        => await context.Users.FindAsync(email);

    public async Task<int> SaveChangesAsync()
        => await context.SaveChangesAsync();

    public async Task<EntityEntry<UserEntity>> AddAsync(UserEntity? email)
        => await context.Users.AddAsync(email);

    public async Task<List<UserEntity>> ToListAsync()
        => await context.Users.ToListAsync();

    public void Remove(UserEntity? userEntity)
        => context.Users.Remove(userEntity);

}