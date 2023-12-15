using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.DAL;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; } //юзер
    public DbSet<EventEntity> Events { get; set; }
    public DbSet<TemplateEntity> Templates { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, Config config) : base(options)
    {
        //
    }
}