﻿using ChronoFlow.API.Infrastructure;

namespace ChronoFlow.API.Modules.UserModule;

public class UserModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddSingleton<PasswordHasher>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IUserRepository, UserRepository>();
        
        return services;
    }
}