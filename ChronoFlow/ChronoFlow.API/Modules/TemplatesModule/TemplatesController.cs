using ChronoFlow.API.DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules.TemplatesModule;

[ApiController]
[Route("api/[controller]")]
public class TemplatesController : ControllerBase
{
    private readonly IUnifyService<TemplateEntity> service;

    public TemplatesController(IUnifyService<TemplateEntity> service)
    {
        this.service = service;
    }

    [HttpGet]
    public Task<ActionResult<IEnumerable<TemplateEntity>>> GetTemplates()
        => service.GetAll();

    [HttpGet("{id:guid}")]
    public Task<ActionResult<TemplateEntity>> GetTemplate([FromRoute] Guid id)
        => service.Get(id);

    [HttpPost]
    public Task<ActionResult<TemplateEntity>> CreateOrUpdateTemplate([FromBody] TemplateEntity templateEntity)
        => service.CreateOrUpdate(templateEntity);

    [HttpDelete("{id:Guid}")]
    public Task<ActionResult> DeleteTemplate([FromRoute] Guid id)
        => service.Delete(id);
}