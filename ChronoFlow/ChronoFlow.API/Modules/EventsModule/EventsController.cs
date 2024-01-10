using AutoMapper;
using ChronoFlow.API.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.EventsModule;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IUnifyService<EventEntity> service;
    private readonly IMapper mapper;

    public EventsController(IUnifyService<EventEntity> service,
        IMapper mapper)
    {
        this.service = service;
        this.mapper = mapper;
    }

    [HttpGet]
    public Task<ActionResult<IEnumerable<EventEntity>>> GetEvents()
    {
        //var res = mapper.Map<EventEntity>(new DateRange() {Start = DateTime.MinValue, End = DateTime.MaxValue});

        return service.GetAll();
    }

    [HttpGet("{id:guid}")]
    public Task<ActionResult<EventEntity>> GetEvent([FromRoute] Guid id)
        => service.Get(id);

    [HttpPost]
    public Task<ActionResult<EventEntity>> CreateOrUpdateEvent([FromBody] EventEntity eventEntity)
        => service.CreateOrUpdate(eventEntity);


    [HttpDelete("{id:Guid}")]
    public Task<ActionResult> DeleteEvent([FromBody] Guid id)
        => service.Delete(id);
}