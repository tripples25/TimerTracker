using AutoMapper;
using ChronoFlow.API.DAL.Entities;

namespace ChronoFlow.API.Modules.EventsModule;

public class EventsMapping : Profile
{
    public EventsMapping()
    {
        CreateMap<EventEntity, EventEntity>();
    }
}