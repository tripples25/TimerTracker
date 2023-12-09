using ChronoFlow.API.Infra;
using ChronoFlow.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.DAL;

public class ApplicationDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Event> Events { get; set; }
    public DbSet<Template> Templates { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, Config config) : base(options)
    {
        
    }
}