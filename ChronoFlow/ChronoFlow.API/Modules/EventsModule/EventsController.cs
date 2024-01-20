using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.EventsModule.Response;
using ChronoFlow.API.Modules.EventsModule.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.EventsModule;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService eventService;
    private readonly IUnifyService<EventEntity> service;

    public EventsController(
        IUnifyService<EventEntity> service,
        IEventService eventService)
    {
        this.service = service;
        this.eventService = eventService;
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

    [HttpPost("{id:Guid}/stopTracking")]
    public Task<ActionResult<EventEntity>> StopTracking([FromRoute] Guid id)
        => eventService.StopTracking(id);

    [HttpPost("addToUser")]
    public Task<ActionResult<UserEntity>> AddUserEvent([FromQuery] string email, [FromQuery] Guid eventGuid)
        => eventService.AddUserEvent(email, eventGuid);

    [HttpDelete("deleteFromUser")]
    public Task<ActionResult<UserEntity>> DeleteUserEvent([FromQuery] string email, [FromQuery] Guid eventGuid)
        => eventService.DeleteUserEvent(email, eventGuid);

    [HttpGet("analytics")]
    public Task<ActionResult<AnalyticsResponse>> GetAnalytics([FromQuery] string email,
        [FromQuery] EventDateFilterRequest requests)
        => eventService.GetAnalytics(email, requests);

    [HttpGet("filter")]
    public Task<ActionResult<IEnumerable<EventDateFilterResponse>>> GetEvents(
        [FromQuery] EventDateFilterRequest request)
        => eventService.GetEvents(request);
}