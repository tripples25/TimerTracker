using ChronoFlow.API.DAL.Entities.Response;
using ChronoFlow.API.Infrastructure;

namespace ChronoFlow.API.Modules.EventsModule;

public class EventModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IEventRepository, EventRepository>();
        
        return services;
    }
}