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
    public async Task<ActionResult<IEnumerable<EventEntity>>> GetEvents([FromRoute] EventEntity searchReq)
    {
        var data = await context.Events.ToListAsync();

        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<EventEntity>> GetSpecificEvent([FromRoute] Guid id)
    {
        var currentEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == id);

        if (currentEvent == null)
            return NotFound();

        return Ok(currentEvent);
    }

    // Желательно писать CreateOrUpdate
    // Или хотя бы не выделять отдельно Patch, а оставлять только POST/PUT
    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromBody] EventEntity evento)
    {
        evento.Id = Guid.Empty;
        await context.Events.AddAsync(evento);
        await context.SaveChangesAsync();

        return Ok(new CreatedOrUpdateResponse
        {
            Id = evento.Id,
        });
    }

    public class CreatedOrUpdateResponse
    {
        public Guid Id { get; set; }
        public bool IsCreated { get; set; }
    }

    [HttpPatch] // лучше было бы [HttpPut]
    public async Task<IActionResult> UpdateEvent([FromRoute] EventEntity evento)
    {
        var eventFromDb = await context.Events.FindAsync(evento.Id);

        if (eventFromDb == null)
            return NotFound();

        eventFromDb.StartTime = evento.StartTime;
        eventFromDb.EndTime = evento.EndTime;

        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:Guid}")]
    public async Task<IActionResult> DeleteEvent([FromRoute] Guid id)
    {
        var eventFromDb = await context.Events.FindAsync(id);

        if (eventFromDb == null)
            return NotFound();

        context.Events.Remove(eventFromDb);
        await context.SaveChangesAsync();

        return NoContent();
    }
}