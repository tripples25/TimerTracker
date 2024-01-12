/*using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ChronoFlow.API.Modules.EventsModule;

public class EventService : ControllerBase, IEventService
{
    private readonly ApplicationDbContext context;
    private readonly IEventRepository eventRepository;
    
    public EventService( IEventRepository eventRepository)
    {
        this.context = context;
        this.eventRepository = eventRepository;
    }

    public async Task<ActionResult<IEnumerable<EventEntity>>> GetEvents()
    {
        var data = await eventRepository.ToListAsync();

        return Ok(data);
    }

    public async Task<ActionResult<EventEntity>> GetEvent(Guid id)
    {
        var currentEvent = await eventRepository.FirstOrDefaultAsync(id);

        if (currentEvent is null)
            return NotFound("The event does not exist");
        return Ok(currentEvent);
    }

    public async Task<ActionResult<EventEntity>> CreateOrUpdateEvenat(EventEntity eventEntity)
    {
        var dbEvent = await eventRepository.FindAsync(eventEntity.Id);
        var isCreated = false;

        if (dbEvent is null)
        {
            eventEntity.Id = Guid.Empty;
            eventEntity.StartTime = eventEntity.StartTime;
            eventEntity.EndTime = eventEntity.EndTime;
            isCreated = true;

            await context.Events.AddAsync(eventEntity);
            await eventRepository.AddAsync(eventEntity);
        }
        else
        {
            dbEvent.StartTime = eventEntity.StartTime;
            dbEvent.EndTime = eventEntity.EndTime;
        }

        await context.SaveChangesAsync();
        await eventRepository.SaveChangesAsync();

        return Ok(new CreateOrUpdateResponse
        {
            Id = eventEntity.Id,
            IsCreated = isCreated
        });
    }

    public async Task<ActionResult> DeleteEvent(Guid id)
    {
        var dbEvent = await eventRepository.FindAsync(id);

        if (dbEvent != null)
        {
            context.Events.Remove(dbEvent);
            await context.SaveChangesAsync();
            eventRepository.Remove(dbEvent);
            await eventRepository.SaveChangesAsync();
        }

        return NoContent();
    }
}*/