using ChronoFlow.API.Infra;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.DAL;

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