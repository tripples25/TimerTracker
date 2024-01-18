using ChronoFlow.API.Modules.EventsModule;

namespace ChronoFlow.API.Modules.TemplatesModule;

public class TemplateDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<EventDto> Events { get; set; } = new List<EventDto>();
}