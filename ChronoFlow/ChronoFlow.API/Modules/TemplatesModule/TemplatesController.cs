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

    // Смотри EventsController
    [HttpGet]
    public async Task<IActionResult> GetTemplates() // Типизорвать ActionResult<Ответ>
    {
        var data = await context.Templates.ToListAsync();

        if (data.Count == 0)
            return NotFound(data); // Не должно быть ошибки. Пустой лист после поиска - Валидное поведение

        return Ok(data);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetSpecificTemplate([FromRoute] TemplateEntity templateEntity)
    {
        var currentTemplate = await context.Templates.FirstOrDefaultAsync(t => t.Id == templateEntity.Id);

        if (currentTemplate == null)
            return NotFound();

        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateTemplate([FromRoute] TemplateEntity templateEntity)
    {
        await context.Templates.AddAsync(templateEntity);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> UpdateTemplate([FromRoute] TemplateEntity templateEntity)
    {
        var templateFromDb = await context.Templates.FindAsync(templateEntity.Id);

        if (templateFromDb == null)
            return NotFound();

        templateFromDb.Name = templateEntity.Name;

        await context.SaveChangesAsync();
        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTemplate([FromRoute] TemplateEntity templateEntity)
    {
        var templateFromDb = await context.Templates.FindAsync(templateEntity.Id);

        if (templateFromDb == null)
            return NotFound();

        context.Templates.Remove(templateEntity);
        await context.SaveChangesAsync();

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> ReplaceTemplate([FromRoute] TemplateEntity templateEntity)
    {
        var templateFromDb = await context.Templates.FindAsync(templateEntity.Id);

        if (templateFromDb == null)
            return NotFound();

        templateFromDb.Id = templateEntity.Id;
        templateFromDb.Name = templateEntity.Name;

        await context.SaveChangesAsync();
        return Ok();
    }
}