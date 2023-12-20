using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.DAL.Entities;

public class TemplateEntity : IEntity<TemplateEntity>
{
    [Key] 
    public Guid Id { get; set; }
    public string Name { get; set; }
    
    //public HashSet<EventEntity> Events { get; set; }
    
    public async void UpdateFieldsFromEntity()
    {
        Id = Guid.Empty;
        Name = string.Empty;
    }

    public async void CreateFieldsFromEntity(TemplateEntity? dbEntity)
    {
        dbEntity.Name = Name;
    }
}