using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Modules.TemplatesModule;

public class TemplateService : ControllerBase, ITemplateService
{
    private readonly ApplicationDbContext context;

    public TemplateService(ApplicationDbContext context)
    {
        this.context = context;
    }


    public async Task<ActionResult<IEnumerable<TemplateEntity>>> GetTemplates()
    {
        var data = await context.Templates.ToListAsync();

        return Ok(data);
    }

    public async Task<ActionResult<TemplateEntity>> GetTemplate(Guid id)
    {
        var currentTemplate = await context.Templates.FirstOrDefaultAsync(t => t.Id == id);

        if (currentTemplate is null)
            return NotFound("The template does not exist");

        return Ok();
    }

    public async Task<ActionResult<TemplateEntity>> CreateOrUpdateTemplate(TemplateEntity templateEntity)
    {
        var dbTemplate = await context.Templates.FindAsync(templateEntity.Id);
        var isCreated = false;

        if (dbTemplate is null)
        {
            templateEntity.Id = Guid.Empty;
            templateEntity.Name = string.Empty;
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

    public async Task<ActionResult<TemplateEntity>> DeleteTemplate(Guid id)
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