using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using ChronoFlow.API.Modules.UserModule.Repository;
using ChronoFlow.API.Modules.UserModule.Requests;
using ChronoFlow.API.Modules.UserModule.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ChronoFlow.API.Modules.EventsModule;

public class EventService : ControllerBase, IEventService
{
    private readonly IUnifyRepository<EventEntity> eventRepository;
    private readonly IUserRepository userRepository;

    public EventService(IUnifyRepository<EventEntity> eventRepository, IUserRepository userRepository)
    {
        this.eventRepository = eventRepository;
        this.userRepository = userRepository;
    }

    public async Task<ActionResult<EventEntity>> StopTracking(Guid eventId)
    {
        var eventDbEntity = await eventRepository.FindAsync(eventId);
        var isNotCreated = eventDbEntity is null;

        if (isNotCreated)
        {
            return NotFound();
        }
        eventDbEntity.EndTime = DateTime.Now;
        await eventRepository.AddAsync(eventDbEntity);


        return Ok();
    }

    public async Task<ActionResult<UserEntity>> AddUserEvent(string email, Guid eventId)
    {
        var user = await userRepository.FindAsync(email);
        var eventEntity = await eventRepository.FindAsync(eventId);
        if (user == null || eventEntity == null)
            return NotFound();

        user.Events.Add(eventEntity);
        await userRepository.SaveChangesAsync();

        return Ok(user);
    }

    public async Task<ActionResult<UserEntity>> DeleteUserEvent(string email, Guid eventId)
    {
        var user = await userRepository.FindAsync(email);
        var eventEntity = await eventRepository.FindAsync(eventId);
        if (user == null)
            return NotFound();

        user.Events.Remove(eventEntity);
        await userRepository.SaveChangesAsync();

        return NoContent();
    }

    public async Task<ActionResult<AnalyticsResponse>> GetAnalytics(string email, UserAnalyticsRequests request)
    {
        var analyticsEventEntity = new HashSet<EventAnalyticsModule>();
        var user = await userRepository.FindAsync(email);
        var events = user.Events
            .Where(d => d.StartTime >= request.Start && d.EndTime <= request.End)
            .GroupBy(n => n.Template.Name);
        int totalHours = default;
        int totalCount = default;
        foreach (var group in events)
        {
            string name = group.Key;
            int timeInMinutes = default;
            int count = group.Count();
            totalCount += count;
            foreach (var e in group)
            {
                timeInMinutes += (int) (e.EndTime - e.StartTime).Value.TotalMinutes;
                totalHours += timeInMinutes;
            }

            analyticsEventEntity.Add
            (
                new EventAnalyticsModule
                (
                    name,
                    timeInMinutes,
                    timeInMinutes / 60,
                    timeInMinutes * 60,
                    count
                )
            );
        }

        return Ok(new AnalyticsResponse(analyticsEventEntity, totalCount, totalHours));
    }
}