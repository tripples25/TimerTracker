using ChronoFlow.API.DAL;

namespace ChronoFlow.API.Infrastructure;

public class ApplicationModule : IModule
{
    //private readonly IConfiguration configuration;

    //public ApplicationModule(IConfiguration configuration)
    //{
    //    this.configuration = configuration;
    //}


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