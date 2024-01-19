using System.Linq.Expressions;
using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.Modules;

public class UnifyRepository<T> : IUnifyRepository<T> 
    where T : class, IEntity<T>
{
    private readonly ApplicationDbContext context;
    private DbSet<T> set => context.Set<T>();
    
    public UnifyRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<T>> ToListAsync(params Expression<Func<T, object>>[] includeExpressions)
    {
        var query = set.AsQueryable();

        foreach (var includeExpression in includeExpressions)
        {
            query = query.Include(includeExpression);
        }

        return await query.ToListAsync();
    }

    public async Task<T> FirstOrDefaultAsync(Guid id, params Expression<Func<T, object>>[] includeExpressions)
    {
        var query = set.AsQueryable();

        foreach (var includeExpression in includeExpressions)
        {
            query = query.Include(includeExpression);
        }

        return await query.FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<T> FindAsync(Guid id)
        => await set.FindAsync(id);

    public void Remove(T entity)
        => set.Remove(entity);

    public async Task<int> SaveChangesAsync()
        => await context.SaveChangesAsync();

    public async Task<EntityEntry<T>> AddAsync(T entity)
        => await set.AddAsync(entity);
}