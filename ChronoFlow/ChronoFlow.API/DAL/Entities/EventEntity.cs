namespace ChronoFlow.API.DAL.Entities;

public class EventEntity : IEntity<EventEntity>
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public UserEntity User { get; set; }
    public TemplateEntity Template { get; set; }
    
    public void UpdateFieldsFromEntity()
    {
        Id = Guid.Empty;
        StartTime = StartTime;
        EndTime = EndTime;
    }

    public void CreateFieldsFromEntity(EventEntity? dbEntity)
    {
        dbEntity.StartTime = StartTime;
        dbEntity.EndTime = EndTime;
    }
}