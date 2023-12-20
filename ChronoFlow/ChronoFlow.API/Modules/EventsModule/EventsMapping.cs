using AutoMapper;
using ChronoFlow.API.DAL.Entities;
using Microsoft.Recognizers.Text.DataTypes.TimexExpression;

namespace ChronoFlow.API.Modules.EventsModule;


public class EventsMapping : Profile
{
    public EventEntity Map(EventEntity @event)
    {
        return new EventEntity
        {
            StartTime = @event.StartTime,
            ...
        };
    }
    
    public EventsMapping()
    {
        // CreateMap<TSource, TDestination>();
        CreateMap<EventEntity, EventEntity>();
        CreateMap<DateRange, EventEntity>()
            .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.Start))
            .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.End));
        
        /*
        CreateMap<EventModel, EventEntity>()
            .ForMember(dest => dest.User, opt => opt.ConvertUsing<EventConverter, Guid>(src => src.UserId));*/
    }
}

/*public class EventConverter : IValueConverter<Guid, UserEntity>
{
    private IUnifyRepository<UserEntity> repo;
    public EventConverter(IUnifyRepository<UserEntity> repo)
    {
        this.repo = repo;
    }
    
    public UserEntity Convert(Guid sourceMember, ResolutionContext context)
    {
        return repo.FindAsync(sourceMember).GetAwaiter().GetResult();
    }
}*/