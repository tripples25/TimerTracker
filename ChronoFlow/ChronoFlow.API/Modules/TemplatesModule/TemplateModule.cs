using ChronoFlow.API.Infrastructure;
using Mosaik.Core;

namespace ChronoFlow.API.Modules.TemplatesModule;

public class TemplateModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddScoped<ITemplateService, TemplateService>();
        services.AddScoped<ITemplateRepository, TemplateRepository>();

        return services;
    }
}