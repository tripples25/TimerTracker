using ChronoFlow.API.DAL;
using Newtonsoft.Json;

namespace ChronoFlow.API.Infrastructure;

public class ApplicationModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore);
        services.AddDbContext<ApplicationDbContext>();
        services.AddAutoMapper(typeof(Program).Assembly);

        return services;
    }
}