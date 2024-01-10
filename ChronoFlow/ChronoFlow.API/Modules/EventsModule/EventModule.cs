using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using ChronoFlow.API.Infrastructure;

namespace ChronoFlow.API.Modules.EventsModule;

public class EventModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IUnifyService<EventEntity>, UnifyService<EventEntity>>();
        services.AddScoped<IUnifyRepository<EventEntity>, UnifyRepository<EventEntity>>();
        services.AddAutoMapper(typeof(EventsMapping));
        // services.AddTransient<EventConverter>();

        return services;
    }
}