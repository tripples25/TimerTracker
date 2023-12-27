using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
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

    public async Task<List<T>> ToListAsync()
    {
        if (typeof(T) == typeof(TemplateEntity))
        {
            return await set.Include(t => (t as TemplateEntity).Events).ToListAsync();
        }
        return await set.Include(t => (t as EventEntity).Template).ToListAsync();
    }
    
    // TODO: На связах обязательно нужен Include(T => T.Field) или у тя просто будут null вместо значений
    // TODO: Можно просто подрубить LazyLoading и не думать о Дозагрузке сущностей
    
    // TODO: Предусмотреть bool Flag or smth который бы рулил за AsNoTracking()
    
    // TODO: Nullabe - убить

    public async Task<T> FirstOrDefaultAsync(Guid id)
    {
        if (typeof(T) == typeof(TemplateEntity))
        {
            return await set.Include(t => (t as TemplateEntity).Events).FirstOrDefaultAsync(e => e.Id == id);
        }
        
        return await set.Include(t => (t as EventEntity).Template).FirstOrDefaultAsync(e => e.Id == id);
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