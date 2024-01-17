namespace ChronoFlow.API.Modules.EventsModule;

public class EventDto
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    public Guid? TemplateId { get; set; }
}