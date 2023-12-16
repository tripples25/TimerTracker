using System.ComponentModel.DataAnnotations;

namespace ChronoFlow.API.DAL.Entities;

public class UserEntity
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    [Key]
    public string Email { get; set; }
    
    public HashSet<EventEntity> Events { get; set; }
}