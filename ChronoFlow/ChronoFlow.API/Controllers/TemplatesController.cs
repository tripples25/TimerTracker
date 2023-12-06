using ChronoFlow.API.DAL;
using ChronoFlow.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public TemplatesController(ApplicationDbContext context)
    {
        this.context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetTemplates()
    {
        var template = new EventTemplate();

        await context.EventTemplates.AddAsync(template);
        await context.SaveChangesAsync();

        var template2 = await context.EventTemplates.FirstOrDefaultAsync(e => e.Name == "1");

        template2.Name = "123123";

        await context.SaveChangesAsync();

        var template3 = await context.EventTemplates.AsNoTracking().FirstOrDefaultAsync(e => e.Name == "1");
        template3.Name = "123";
        await context.SaveChangesAsync();

        context.EventTemplates.Remove(template3);

        await context.SaveChangesAsync();

        var data = await context.EventTemplates.ToListAsync();

        return Ok(data);
    }

    [HttpPost]
    public async Task<EventTemplate> CreateTemplate([FromQuery] string template)
    {
        return new EventTemplate();
    }

    [HttpPatch]
    public async Task<EventTemplate> UpdateEvent([FromQuery] string template)
    {
        return new EventTemplate();
    }
}