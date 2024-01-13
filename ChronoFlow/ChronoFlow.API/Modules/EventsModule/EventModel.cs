namespace ChronoFlow.API.Modules.EventsModule;

public class EventModel
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid UserId { get; set; }
}