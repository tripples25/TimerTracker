using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.EventsModule.Response;
using ChronoFlow.API.Modules.EventsModule.Responses;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.EventsModule;

public interface IEventService
{
    Task<ActionResult<EventEntity>> StopTracking([FromRoute] Guid eventId);
    Task<ActionResult<UserEntity>> AddUserEvent(string email, Guid eventId);
    Task<ActionResult<UserEntity>> DeleteUserEvent(string email, Guid eventId);
    Task<ActionResult<AnalyticsResponse>> GetAnalytics(string email, EventDateFilterRequest request);
    Task<ActionResult<IEnumerable<EventDateFilterResponse>>> GetEvents(EventDateFilterRequest request);
}