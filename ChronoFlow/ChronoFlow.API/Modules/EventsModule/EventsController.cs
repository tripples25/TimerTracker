using ChronoFlow.API.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.EventsModule;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IUnifyService<EventEntity> service;

    public EventsController(IUnifyService<EventEntity> service)
    {
        this.service = service;
    }

    [HttpGet]
    public Task<ActionResult<IEnumerable<EventEntity>>> GetEvents()
        => service.GetAll();

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