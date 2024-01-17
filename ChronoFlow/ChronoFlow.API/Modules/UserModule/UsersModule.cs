using ChronoFlow.API.Infrastructure;
using ChronoFlow.API.Modules.UserModule.Repository;
using ChronoFlow.API.Modules.UserModule.Service;

namespace ChronoFlow.API.Modules.UserModule;

public class UserModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddSingleton<PasswordHasher>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddAutoMapper(typeof(UsersMapping));
        
        return services;
    }
}