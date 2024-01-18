using AutoMapper;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.EventsModule;
using ChronoFlow.API.Modules.TemplatesModule;

namespace ChronoFlow.API.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        /*
        CreateMap<EventEntity, EventDto>()
            .ForMember(dest => dest.TemplateId, opt => opt.MapFrom(src => src.Template.Id))
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartTime))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndTime));

        CreateMap<TemplateEntity, TemplateDto>()
            .ForMember(dest => dest.Events, opt => opt.MapFrom(src => src.Events))
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));
            */
        
        CreateMap<EventEntity, EventDto>().ReverseMap();
        CreateMap<TemplateEntity, TemplateDto>().ReverseMap();
        
    }
}