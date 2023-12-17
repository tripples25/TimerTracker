using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules;

public abstract class BaseCRUDController<T> : ControllerBase
{
    private readonly CRUDRepository<T> repo;
    
    [HttpGet]
    public ActionResult<T> Test()
    {
        return Ok(repo.Get());
    }
}
// Include должен работать
// Expression может сильно помочь с обработкой кастомного Include
public abstract class CRUDRepository<T>
{
    public T? Get() => default(T);
}

public class EventsRepo : CRUDRepository<EventEntity>
{
};

[ApiController]
[Route("api/[controller]")]
public class RealContoller : BaseCRUDController<EventEntity>{};

public class BaseModule : IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services)
    {
        services.AddScoped<CRUDRepository<EventEntity>, EventsRepo>();
        
        return services;
    }
}