using AutoMapper;

namespace ChronoFlow.API.Modules;

public static class AutoMapperExtensions
{
    public static TDestination Map<TSource, TDestination>(this IMapper mapper, TSource source, TDestination destination,
        bool isCreated)
    {
        return mapper.Map(source, destination, opts 
            => opts.Items["IsCreated"] = isCreated);
    }
}