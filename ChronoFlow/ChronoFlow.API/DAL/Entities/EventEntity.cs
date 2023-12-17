namespace ChronoFlow.API.DAL.Entities;

public class EventEntity : IEntity
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; } public DateTime? EndTime { get; set; }
    public UserEntity User { get; set; }
    public TemplateEntity Template { get; set; }
}