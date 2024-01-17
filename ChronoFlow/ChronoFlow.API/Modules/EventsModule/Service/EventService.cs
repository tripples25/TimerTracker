using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ChronoFlow.API.Modules.EventsModule;

public class EventService : ControllerBase, IEventService
{
    private readonly IUnifyRepository<EventEntity> eventRepository;

    public EventService(IUnifyRepository<EventEntity> eventRepository)
    {
        this.eventRepository = eventRepository;
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


}