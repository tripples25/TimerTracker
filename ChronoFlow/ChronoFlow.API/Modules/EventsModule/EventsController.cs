using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Modules.EventsModule;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public EventsController(ApplicationDbContext context)
    {
        this.context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventEntity>>> GetEvents()
    {
        var data = await context.Events.ToListAsync();

        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EventEntity>> GetSpecificEvent([FromRoute] Guid id)
    {
        var currentEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == id);

        if (currentEvent is null)
            return NotFound();

        return Ok(currentEvent);
    }
    
    [HttpPost]
    public async Task<ActionResult<EventEntity>> CreateOrUpdateEvent([FromBody] EventEntity eventEntity)
    {
        var dbEvent = await context.Events.FindAsync(eventEntity.Id);
        var isCreated = false;

        if (dbEvent is null)
        {
            eventEntity.Id = Guid.Empty;
            eventEntity.StartTime = new DateTime();
            eventEntity.EndTime = new DateTime();
            isCreated = true;

            await context.Events.AddAsync(eventEntity);
        }
        else
        {
            dbEvent.StartTime = eventEntity.StartTime;
            dbEvent.EndTime = eventEntity.EndTime;
        }

        await context.SaveChangesAsync();
        
        return Ok(new CreateOrUpdateResponse
        {
            Id = eventEntity.Id,
            IsCreated = isCreated
        });
    }

    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<EventEntity>> DeleteEvent([FromBody] Guid id)
    {
        var dbEvent = await context.Events.FindAsync(id);

        if (dbEvent != null)
        {
            context.Events.Remove(dbEvent);
            await context.SaveChangesAsync();
        }
        
        return NoContent();
    }
}