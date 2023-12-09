using System.Xml.Linq;
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
    
    [HttpGet]
    public async Task<IActionResult> GetEvent()
    {
        var data = await context.Events.ToListAsync();
        
        if (data.Count == 0)
            return NotFound(data);

        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSpecificEvent([FromRoute] Event evento)
    {
        var currentEvent = await context.Events.FirstOrDefaultAsync(e => e.Id == evento.Id);
        
        if (currentEvent == null)
            return NotFound();
        
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateEvent([FromRoute] Event evento)
    {
        await context.Events.AddAsync(evento);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateEvent([FromRoute] Event evento)
    {
        var eventFromDb = await context.Events.FindAsync(evento.Id);
        
        if (eventFromDb == null)
            return NotFound();

        eventFromDb.StartTime = evento.StartTime;
        eventFromDb.EndTime = evento.EndTime;

        await context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteEvent([FromRoute] Event evento)
    {
        var eventFromDb = await context.Events.FindAsync(evento.Id);
        
        if (eventFromDb == null)
            return NotFound();
        
        context.Events.Remove(evento);
        await context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> ReplaceEvent([FromRoute] Event evento)
    {
        var eventFromDb = await context.Events.FindAsync(evento.Id);
        
        if (eventFromDb == null)
            return NotFound();
        
        eventFromDb.Id = evento.Id;
        eventFromDb.StartTime = evento.StartTime;
        eventFromDb.EndTime = evento.EndTime;
        
        await context.SaveChangesAsync();
        return Ok();
    }
}