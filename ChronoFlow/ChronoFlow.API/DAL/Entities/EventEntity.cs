using AutoMapper.Configuration.Annotations;
using Newtonsoft.Json;

namespace ChronoFlow.API.DAL.Entities;

public class EventEntity : IEntity<EventEntity>
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public Guid? TemplateId { get; set; }
    public string UserEmail { get; set; }
    [Ignore, JsonIgnore]
    public virtual UserEntity? User { get; } 
    [Ignore, JsonIgnore]
    public virtual TemplateEntity? Template { get; }
    
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