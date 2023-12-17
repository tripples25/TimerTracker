using ChronoFlow.API.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.EventsModule;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService service;

    public EventsController(IEventService service)
    {
        this.service = service;
    }

    [HttpGet]
    public Task<ActionResult<IEnumerable<EventEntity>>> GetEvents()
        => service.GetEvents();

    [HttpGet("{id:guid}")]
    public Task<ActionResult<EventEntity>> GetEvent([FromRoute] Guid id)
        => service.GetEvent(id);

    [HttpPost]
    public Task<ActionResult<EventEntity>> CreateOrUpdateEvent([FromBody] EventEntity eventEntity)
        => service.CreateOrUpdateEvent(eventEntity);


    [HttpDelete("{id:Guid}")]
    public Task<ActionResult> DeleteEvent([FromBody] Guid id)
        => service.DeleteEvent(id);
}