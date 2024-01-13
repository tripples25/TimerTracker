using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.DAL.Entities;

public class UserEntity
{
    [Key]
    public string Email { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }

    public virtual HashSet<EventEntity> Events { get; set; }
}