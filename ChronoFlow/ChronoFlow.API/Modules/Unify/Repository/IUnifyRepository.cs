using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.Modules;

public interface IUnifyRepository<T> where T : class
{
    public Task<List<T>> ToListAsync(params Expression<Func<T, object>>[] includeExpressions);
    public Task<T> FirstOrDefaultAsync(Guid id, params Expression<Func<T, object>>[] includeExpressions);
    public Task<T> FindAsync(Guid id);
    public void Remove(T entity);
    public Task<int> SaveChangesAsync();
    public Task<EntityEntry<T>> AddAsync(T entity);
}