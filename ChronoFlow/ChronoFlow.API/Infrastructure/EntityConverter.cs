using AutoMapper;
using ChronoFlow.API.DAL.Entities;

namespace ChronoFlow.API.Infrastructure;

public class EntityConverter : ITypeConverter<IEntity<object>, object>
{
    public object Convert(IEntity<object> source, object destination, ResolutionContext context)
    {
        

        return source;
    }
}