using ChronoFlow.API.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.EventsModule;

public interface IEventService
{
    Task<ActionResult<EventEntity>> StopTracking([FromRoute] Guid eventId);
}