using ChronoFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.DAL;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<EventTemplate> EventTemplates { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
}