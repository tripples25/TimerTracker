using AutoMapper;
using ChronoFlow.API.DAL.Entities;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace ChronoFlow.API.Modules.EventsModule;

public class EventsMapping : Profile
{
    public EventsMapping()
    {
        CreateMap<EventEntity, EventEntity>();
        /*.ForMember(dest => dest.TemplateId,
            opt => opt.MapFrom(src => src.TemplateId))
        .ForMember(dest => dest.StartTime,
            opt => opt.MapFrom(src => src.StartTime))
        .ForMember(dest => dest.EndTime,
            opt => opt.MapFrom(src => src.EndTime));*/

    }
}