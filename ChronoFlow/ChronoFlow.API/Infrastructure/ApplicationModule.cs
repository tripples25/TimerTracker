using ChronoFlow.API.DAL;
using ChronoFlow.API.Modules.EventsModule;
using ChronoFlow.API.Modules.TemplatesModule;
using ChronoFlow.API.Modules.UserModule;

namespace ChronoFlow.API.Infrastructure;

public class ApplicationModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddControllers().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        //services.AddSingleton(configuration);
        services.AddDbContext<ApplicationDbContext>();

        return services;
    }
}