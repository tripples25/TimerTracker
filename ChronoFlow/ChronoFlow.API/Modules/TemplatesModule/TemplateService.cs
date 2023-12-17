using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.Modules.TemplatesModule;

public class TemplateService : ControllerBase, ITemplateService
{
    private readonly ITemplateRepository templateRepository;

    public TemplateService(ITemplateRepository templateRepository)
    {
        this.templateRepository = templateRepository;
    }


    public async Task<ActionResult<IEnumerable<TemplateEntity>>> GetTemplates()
    {
        var data = await templateRepository.ToListAsync();

        return Ok(data);
    }

    public async Task<ActionResult<TemplateEntity>> GetTemplate(Guid id)
    {
        var currentTemplate = await templateRepository.FirstOrDefaultAsync(id);

        if (currentTemplate is null)
            return NotFound("The template does not exist");

        return Ok();
    }

    public async Task<ActionResult<TemplateEntity>> CreateOrUpdateTemplate(TemplateEntity templateEntity)
    {
        var dbTemplate = await templateRepository.FindAsync(templateEntity.Id);
        var isCreated = false;

        if (dbTemplate is null)
        {
            templateEntity.Id = Guid.Empty;
            templateEntity.Name = string.Empty;
            isCreated = true;
            await templateRepository.AddAsync(templateEntity);
        }
        else
        {
            dbTemplate.Name = templateEntity.Name;
        }

        await templateRepository.SaveChangesAsync();

        return Ok(new CreateOrUpdateResponse
        {
            Id = templateEntity.Id,
            IsCreated = isCreated,
        });
    }

    public async Task<ActionResult> DeleteTemplate(Guid id)
    {
        var dbTemplate = await templateRepository.FindAsync(id);
        
        if (dbTemplate != null)
        {
            templateRepository.Remove(dbTemplate);
            await templateRepository.SaveChangesAsync();
        }

        return NoContent();
    }
}