using ChronoFlow.API.Infrastructure;

namespace ChronoFlow.API.Modules.UserModule;

public class UsersModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddSingleton<PasswordHasher>();
        services.AddScoped<IUsersService, UsersService>();
        services.AddScoped<IUsersRepository, UsersRepository>();
        
        return services;
    }
}