using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.DAL.Entities;

public class TemplateEntity : IEntity
{
    [Key] 
    public Guid Id { get; set; }
    public string Name { get; set; }
}