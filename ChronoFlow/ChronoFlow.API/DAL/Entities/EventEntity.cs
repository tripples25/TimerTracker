using System.Text.Json.Serialization;
using AutoMapper.Configuration.Annotations;

namespace ChronoFlow.API.DAL.Entities;

public class EventEntity : IEntity<EventEntity>
{
    public Guid Id { get; set; } = Guid.Empty;
    public DateTime StartTime { get; set; } = DateTime.Now;
    [JsonIgnore]
    public DateTime? EndTime { get; set; } = default;

    [Ignore, JsonIgnore]
    public virtual UserEntity? User { get; set; }
    public virtual TemplateEntity? Template { get; set; }

    public Guid? TemplateId { get; set; } = Guid.Empty;
    public string? UserEmail { get; set; } = "emptyname";
}