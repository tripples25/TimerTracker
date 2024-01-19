using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.DAL.Entities;

public interface IEntity<T>
{
    [Key]
    public Guid Id { get; set; }
    void Update(T? dbEntity);
    void Create(T? dbEntity);
}