/*using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.DAL.Entities.Response;

public interface IEventRepository
{
    public Task<List<EventEntity?>> ToListAsync();
    public Task<EventEntity?> FirstOrDefaultAsync(Guid id);
    public Task<EventEntity?> FindAsync(Guid id);
    public void Remove(EventEntity? entity);
    public Task<int> SaveChangesAsync();
    public Task<EntityEntry<EventEntity>> AddAsync(EventEntity? entity);
}*/