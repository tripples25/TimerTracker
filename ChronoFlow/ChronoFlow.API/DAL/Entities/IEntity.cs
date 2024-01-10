using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.DAL.Entities;

public interface IEntity
{
    [Key]
    public Guid Id { get; set; }
}