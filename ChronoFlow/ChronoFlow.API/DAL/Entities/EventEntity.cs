namespace ChronoFlow.API.Models;

public class EventEntity : IEntity
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
}