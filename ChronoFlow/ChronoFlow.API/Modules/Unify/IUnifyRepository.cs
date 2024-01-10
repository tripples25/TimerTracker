using ChronoFlow.API.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.Modules;

public interface IUnifyRepository<T> where T : class
{
    public Task<List<T>> ToListAsync();
    public Task<T> FirstOrDefaultAsync(Guid id);
    public Task<T> FindAsync(Guid id);
    public void Remove(T entity);
    public Task<int> SaveChangesAsync();
    public Task<EntityEntry<T>> AddAsync(T entity);
}