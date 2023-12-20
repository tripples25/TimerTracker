using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.Modules;

public class UnifyRepository<T> : IUnifyRepository<T> where T : class, IEntity<T>
{
    private readonly ApplicationDbContext context;

    protected UnifyRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<T?>> ToListAsync()
        => await context.Set<T?>().ToListAsync();
    
    public async Task<T?> FirstOrDefaultAsync(Guid id)
        => await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

    public async Task<T?> FindAsync(Guid id)
        => await context.Set<T>().FindAsync(id);

    public void Remove(T? entity)
        => context.Set<T?>().Remove(entity);

    public async Task<int> SaveChangesAsync()
        => await context.SaveChangesAsync();

    public async Task<EntityEntry<T>> AddAsync(T? entity)
        => await context.Set<T?>().AddAsync(entity);
}