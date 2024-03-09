using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.DAL;

public class ApplicationDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; set; }
    public DbSet<EventEntity> Events { get; set; }
    public DbSet<TemplateEntity> Templates { get; set; }
    private readonly Config config;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, Config config) : base(options)
    {
        this.config = config;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        optionsBuilder
            .UseLazyLoadingProxies()
            .UseNpgsql(config.DatabaseConnectionString,
                builder => { builder.
                    EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null); });
        base.OnConfiguring(optionsBuilder);
    }
}