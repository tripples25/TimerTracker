using ChronoFlow.API.DAL;
using ChronoFlow.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventsController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public EventsController(ApplicationDbContext context)
    {
        this.context = context;
    }

    // Должно быть в модуле
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventEntity>>> GetEvents()
    {
        var data = await context.Events.ToListAsync();
        
        if (data.Count == 0)
            return NotFound(data);

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

    // Желательно писать CreateOrUpdate
    // Или хотя бы не выделять отдельно Patch, а оставлять только POST/PUT
    [HttpPost]
    public async Task<ActionResult<EventEntity>> CreateOrUpdateEvent([FromBody] EventEntity eventEntity)
    {
        var dbEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == eventEntity.Id);

        if (dbEvent is null)
        {
            eventEntity.Id = Guid.Empty;
            eventEntity.StartTime = new DateTime();
            eventEntity.EndTime = new DateTime();

            await context.Events.AddAsync(eventEntity);
            await context.SaveChangesAsync();

            return Ok(new CreateOrUpdateResponse
            {
                Id = eventEntity.Id,
                IsCreated = true
            });
        }

        dbEvent.Id = eventEntity.Id;
        dbEvent.StartTime = eventEntity.StartTime;
        dbEvent.EndTime = eventEntity.EndTime;

        await context.Events.AddAsync(dbEvent);
        await context.SaveChangesAsync();

        return Ok(new CreateOrUpdateResponse
        {
            Id = eventEntity.Id,
            IsCreated = false
        });
    }

    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<EventEntity>> DeleteEvent([FromBody] Guid id)
    {
        var dbEvent = await context.Events.FindAsync(id);

        if (dbEvent == null)
            return NotFound();

        context.Events.Remove(dbEvent);
        await context.SaveChangesAsync();

        return NoContent();
    }
}