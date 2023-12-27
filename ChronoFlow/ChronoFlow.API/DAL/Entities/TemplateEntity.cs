using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.DAL.Entities;

public class TemplateEntity : IEntity<TemplateEntity>
{
    [Key] 
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    public virtual List<EventEntity> Events { get; set; }
    
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