using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Modules.UserModule.Requests;
using ChronoFlow.API.Modules.UserModule.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.EventsModule;

public interface IEventService
{
    Task<ActionResult<EventEntity>> StopTracking([FromRoute] Guid eventId);
    Task<ActionResult<UserEntity>> AddUserEvent(string email, Guid eventId);
    Task<ActionResult<UserEntity>> DeleteUserEvent(string email, Guid eventId);
    Task<ActionResult<AnalyticsResponse>> GetAnalytics(string email, UserAnalyticsRequests request);
}