using AutoMapper.Configuration.Annotations;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ChronoFlow.API.DAL.Entities;

public class EventEntity : IEntity<EventEntity>
{
    public Guid Id { get; set; }
    public DateTime StartTime { get; set; }
    [JsonIgnore]
    public DateTime? EndTime { get; set; }

    [Ignore, JsonIgnore]
    public virtual UserEntity? User { get; set; } 
    public virtual TemplateEntity? Template { get; set;  }
    
    public Guid? TemplateId { get; set; }
    public string? UserEmail { get; set; }
    
    public void UpdateFieldsFromEntity(EventEntity? dbEntity)
    {
        dbEntity.TemplateId = TemplateId;
    }

    public void CreateFieldsFromEntity(EventEntity? dbEntity)
    {
        StartTime = DateTime.Now;
        EndTime = default;
        TemplateId = Guid.Empty;
    }
}