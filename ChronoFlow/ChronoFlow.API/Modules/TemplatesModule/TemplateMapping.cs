using AutoMapper;
using ChronoFlow.API.DAL.Entities;

namespace ChronoFlow.API.Modules.TemplatesModule;

public class TemplateMapping : Profile
{
    public TemplateMapping()
    {
        CreateMap<TemplateEntity, TemplateEntity>();
    }
}