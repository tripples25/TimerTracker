using System.ComponentModel.DataAnnotations;
using AutoMapper.Configuration.Annotations;
using Newtonsoft.Json;

namespace ChronoFlow.API.DAL.Entities;

public class TemplateEntity : IEntity<TemplateEntity>
{
    [Key] 
    public Guid Id { get; set; }
    public string Name { get; set; }
    [Ignore]
    public virtual List<EventEntity> Events { get; } = new List<EventEntity>();
    
    public void UpdateFieldsFromEntity()
    {
        Id = Guid.Empty;
        Name = string.Empty;
    }

    public void CreateFieldsFromEntity(TemplateEntity? dbEntity)
    {
        dbEntity.Name = Name;
    }
}