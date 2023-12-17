using ChronoFlow.API.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.TemplatesModule;

[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly ITemplateService service;

    public TemplatesController(ITemplateService service)
    {
        this.service = service;
    }

    [HttpGet]
    public Task<ActionResult<IEnumerable<TemplateEntity>>> GetTemplates()
        => service.GetTemplates();

    [HttpGet("{id:guid}")]
    public Task<ActionResult<TemplateEntity>> GetTemplate([FromRoute] Guid id)
        => service.GetTemplate(id);

    [HttpPost]
    public Task<ActionResult<TemplateEntity>> CreateOrUpdateTemplate([FromBody] TemplateEntity templateEntity)
        => service.CreateOrUpdateTemplate(templateEntity);

    [HttpDelete("{id:Guid}")]
    public Task<ActionResult> DeleteTemplate([FromRoute] Guid id)
        => service.DeleteTemplate(id);
}