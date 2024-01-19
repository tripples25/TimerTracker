namespace ChronoFlow.API.Modules.EventsModule;

public class EventDto
{
    public Guid TemplateId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
}