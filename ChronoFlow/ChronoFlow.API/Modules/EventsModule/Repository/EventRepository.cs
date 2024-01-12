/*using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.Modules.EventsModule;

public class EventRepository : IEventRepository
{
    private readonly ApplicationDbContext context;

    public EventRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<EventEntity>> ToListAsync()
        => await context.Events.ToListAsync();

    public async Task<EventEntity?> FirstOrDefaultAsync(Guid id)
        => await context.Events.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<EventEntity?> FindAsync(Guid id)
        => await context.Events.FindAsync(id);

    public void Remove(EventEntity? entity)
        => context.Events.Remove(entity);

    public async Task<int> SaveChangesAsync()
        => await context.SaveChangesAsync();

    public async Task<EntityEntry<EventEntity>> AddAsync(EventEntity? entity)
        => await context.Events.AddAsync(entity);
}*/