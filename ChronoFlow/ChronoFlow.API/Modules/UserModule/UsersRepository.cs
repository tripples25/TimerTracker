using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Modules.UserModule;

public interface IUsersRepository
{
    Task<UserEntity?> FindAsync(string email);
}

public class UsersRepository : IUsersRepository
{
    private readonly ApplicationDbContext context;

    public UsersRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<UserEntity?> FindAsync(string email)
        => await context.Users.FindAsync(email);
}