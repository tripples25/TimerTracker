using ChronoFlow.API.DAL;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Infrastructure;

public class ApplicationModule : IModule
{
    public ApplicationModule()
    {
    }

    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddControllers();
        //services.AddSingleton(configuration);
        services.AddDbContext<ApplicationDbContext>();

        return services;
    }
}