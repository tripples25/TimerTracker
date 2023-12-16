using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Modules.TemplatesModule;

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

        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TemplateEntity>> GetTemplateAsync([FromRoute] Guid id)
    {
        var currentTemplate = await context.Templates.FirstOrDefaultAsync(t => t.Id == id);

        if (currentTemplate is null)
            return NotFound();

        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<TemplateEntity>> CreateOrUpdateTemplate([FromBody] TemplateEntity templateEntity)
    {
        var dbTemplate = await context.Templates.FindAsync(templateEntity.Id);
        var isCreated = false;

        if (dbTemplate is null)
        {
            templateEntity.Id = Guid.Empty;
            templateEntity.Name = "";
            isCreated = true;
            await context.AddAsync(templateEntity);
        }
        else
        {
            dbTemplate.Name = templateEntity.Name;
        }

        await context.SaveChangesAsync();

        return Ok(new CreateOrUpdateResponse
        {
            Id = templateEntity.Id,
            IsCreated = isCreated,
        });
    }

    [HttpDelete("{id:Guid}")]
    public async Task<ActionResult<TemplateEntity>> DeleteTemplate([FromRoute] Guid id)
    {
        var dbTemplate = await context.Templates.FindAsync(id);
        if (dbTemplate != null)
        {
            context.Templates.Remove(dbTemplate);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }
}