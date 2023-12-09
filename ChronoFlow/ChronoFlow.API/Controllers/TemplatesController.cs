using Azure.Core;
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
        /*var template = new Template();

        await context.Templates.AddAsync(template);
        await context.SaveChangesAsync();

        var template2 = await context.Templates.FirstOrDefaultAsync(e => e.Name == "1");

        template2.Name = "123123";

        await context.SaveChangesAsync();

        var template3 = await context.Templates.AsNoTracking().FirstOrDefaultAsync(e => e.Name == "1");
        template3.Name = "123";
        await context.SaveChangesAsync();

        context.Templates.Remove(template3);

        await context.SaveChangesAsync();*/

        var data = await context.Templates.ToListAsync();
        
        if (data.Count == 0)
            return NotFound(data);

        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSpecificTemplate([FromRoute] Template template)
    {
        var currentTemplate = await context.Templates.FirstOrDefaultAsync(t => t.Id == template.Id);
        
        if (currentTemplate == null)
            return NotFound();
        
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromRoute] Template template)
    {
        await context.Templates.AddAsync(template);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateTemplate([FromRoute] Template template)
    {
        var templateFromDb = await context.Templates.FindAsync(template.Id);
        
        if (templateFromDb == null)
            return NotFound();

        templateFromDb.Name = template.Name;

        await context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteTemplate([FromRoute] Template template)
    {
        var templateFromDb = await context.Templates.FindAsync(template.Id);
        
        if (templateFromDb == null)
            return NotFound();
        
        context.Templates.Remove(template);
        await context.SaveChangesAsync();
        
        return Ok();
    }
    
    [HttpPut]
    public async Task<IActionResult> ReplaceTemplate([FromRoute] Template template)
    {
        var templateFromDb = await context.Templates.FindAsync(template.Id);
        
        if (templateFromDb == null)
            return NotFound();
        
        templateFromDb.Id = template.Id;
        templateFromDb.Name = template.Name;
        
        await context.SaveChangesAsync();
        return Ok();
    }
}