using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;

namespace ChronoFlow.API.Modules.UserModule;

public interface IUsersRepository
{
    Task<UserEntity?> FindAsync(string email);
    //Task<UserEntity?> FindAsync(Guid id);
}

public class UserRepository : IUsersRepository
{
    private readonly ApplicationDbContext context;

    public UserRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<UserEntity?> FindAsync(string email)
        => await context.Users.FindAsync(email);
    
    /*public async Task<UserEntity?> FindAsync(Guid id)
        => await context.Users.FindAsync(id);*/
}