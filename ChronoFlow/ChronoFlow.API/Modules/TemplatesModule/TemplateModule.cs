using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Infrastructure;
using Mosaik.Core;

namespace ChronoFlow.API.Modules.TemplatesModule;

public class TemplateModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddScoped<IUnifyService<TemplateEntity>, UnifyService<TemplateEntity>>();
        services.AddScoped<IUnifyRepository<TemplateEntity>, UnifyRepository<TemplateEntity>>();
        services.AddAutoMapper(typeof(TemplateMapping));

        return services;
    }
}