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

    /*public void RegisterModule(IServiceCollection services)
    {
        services.AddSingleton(configuration);

        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        
        var config = new Config(true);
        services.AddSingleton(new Config(true));
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(config.DatabaseConnectionString));
        /*services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("TimeTrackerDB")));#1#
        
        services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Events.OnRedirectToLogin = (context) =>
                {
                    context.Response.StatusCode = 401; // обработать ещё 403 ошибку
                    return Task.CompletedTask;
                };
            });
        services.AddAuthorization();    
    }*/
    
}