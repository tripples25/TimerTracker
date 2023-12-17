using ChronoFlow.API.Infrastructure;

namespace ChronoFlow.API.Modules.UserModule;

public class UsersModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddSingleton<PasswordHasher>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUsersRepository, UserRepository>();
        
        return services;
    }
}