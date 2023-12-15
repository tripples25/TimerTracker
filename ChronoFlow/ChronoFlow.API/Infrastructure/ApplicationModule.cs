using ChronoFlow.API.DAL;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Infrastructure;

public class ApplicationModule : IModule
{
    private readonly IConfiguration configuration;

    public ApplicationModule(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddControllers();
        services.AddSingleton(configuration);
        services.AddSingleton(new Config(true));
        services.AddDbContext<ApplicationDbContext>();

        return services;
    }
    
}