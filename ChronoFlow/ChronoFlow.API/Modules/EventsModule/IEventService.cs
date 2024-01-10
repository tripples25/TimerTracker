using ChronoFlow.API.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.EventsModule;

public interface IEventService
{
    Task<ActionResult<IEnumerable<EventEntity>>> GetEvents();
    Task<ActionResult<EventEntity>> GetEvent([FromRoute] Guid id);
    Task<ActionResult<EventEntity>> CreateOrUpdateEvent([FromBody] EventEntity eventEntity);
    Task<ActionResult> DeleteEvent([FromBody] Guid id);
}