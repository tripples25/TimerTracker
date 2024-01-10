using AutoMapper.Configuration.Annotations;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class EventEntity : IEntity
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    [Ignore, JsonIgnore]
    public virtual UserEntity? User { get; set; } 
    [Ignore, JsonIgnore]
    public virtual TemplateEntity? Template { get; set;  }
    
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