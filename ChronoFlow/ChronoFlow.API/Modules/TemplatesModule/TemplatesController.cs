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
    public async Task<ActionResult<IEnumerable<TemplateEntity>>> GetTemplates()
    {
        var data = await context.Templates.ToListAsync();

        if (data.Count == 0)
            return NotFound(data);

        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TemplateEntity>> GetSpecificTemplate([FromRoute] Guid id)
    {
        var currentTemplate = await context.Templates.FirstOrDefaultAsync(t => t.Id == id);

        if (currentTemplate is null)
            return NotFound();

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<TemplateEntity>> CreateOrUpdateTemplate([FromBody] TemplateEntity templateEntity)
    {
        var dbTemplate = await context.Templates.FirstOrDefaultAsync(t => t.Id == templateEntity.Id);

        if (dbTemplate is null)
        {
            templateEntity.Id = Guid.Empty;
            templateEntity.Name = "";
            
            await context.Templates.AddAsync(templateEntity);
            await context.SaveChangesAsync();
            
            return Ok(new CreateOrUpdateResponse
            {
                Id = templateEntity.Id,
                IsCreated = true
            });
        }
        
        dbTemplate.Id = templateEntity.Id;
        dbTemplate.Name = templateEntity.Name;
        
        await context.Templates.AddAsync(templateEntity);
        await context.SaveChangesAsync();

        return Ok(new CreateOrUpdateResponse
        {
            Id = templateEntity.Id,
            IsCreated = false
        });
    }

    [HttpDelete]
    public async Task<ActionResult<TemplateEntity>> DeleteTemplate([FromBody] TemplateEntity templateEntity)
    {
        var dbTemplate = await context.Templates.FindAsync(templateEntity.Id);

        if (dbTemplate == null)
            return NotFound();

        context.Templates.Remove(templateEntity);
        await context.SaveChangesAsync();

        return NoContent();
    }
}