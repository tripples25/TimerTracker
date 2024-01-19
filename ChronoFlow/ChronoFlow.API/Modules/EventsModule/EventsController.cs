using AutoMapper;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.UserModule.Requests;
using ChronoFlow.API.Modules.UserModule.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.EventsModule;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{   
    private readonly IEventService eventService;
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


    /*public Task<ActionResult<IEnumerable<EventDto>>> GetAllFiltered()
    {
        var events = service.GetAll();
        var eventsDto = mapper.Map<List<EventDto>>(events);
        return Ok(eventsDto);
    }*/

    [HttpPost("{id:Guid}/stopTracking")]
    public Task<ActionResult<EventEntity>> StopTracking([FromRoute] Guid id)
        => eventService.StopTracking(id);

    [HttpPost("/AddToUser/")]
    public Task<ActionResult<UserEntity>> AddUserEvent([FromQuery] string email, [FromQuery] Guid eventGuid)
        => eventService.AddUserEvent(email, eventGuid);

    [HttpDelete("/DeleteFromUser/")]
    public Task<ActionResult<UserEntity>> DeleteUserEvent([FromQuery] string email, [FromQuery] Guid eventGuid)
        => eventService.DeleteUserEvent(email, eventGuid);

    [HttpGet("/analytics/")]
    public Task<ActionResult<AnalyticsResponse>> GetAnalytics([FromQuery] string email, [FromQuery] UserAnalyticsRequests requests)
        => eventService.GetAnalytics(email, requests);
}