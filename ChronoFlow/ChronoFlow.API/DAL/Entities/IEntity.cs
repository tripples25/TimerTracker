using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.Models;

public interface IEntity
{
    [Key]
    public Guid Id { get; set; }
}