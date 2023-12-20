using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.Modules;

public class UnifyRepository<T> : IUnifyRepository<T> 
    where T : class, IEntity<T>
{
    private readonly ApplicationDbContext context;
    private DbSet<T> Set => context.Set<T>();

    // TODO: Не собирался, тк конструктор был protected
    public UnifyRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<T?>> ToListAsync()
        => await Set.ToListAsync(); // TODO: На Set перейти
    
    // TODO: На связах обязательно нужен Include(T => T.Field) или у тя просто будут null вместо значений
    // TODO: Можно просто подрубить LazyLoading и не думать о Дозагрузке сущностей
    
    // TODO: Предусмотреть bool Flag or smth который бы рулил за AsNoTracking()
    
    // TODO: Nullabe - убить
    
    public async Task<T?> FirstOrDefaultAsync(Guid id)
        => await context.Set<T>().FirstOrDefaultAsync(e => e.Id == id);

    public async Task<T?> FindAsync(Guid id)
        => await context.Set<T>().FindAsync(id);

    public void Remove(T entity)
        => context.Set<T>().Remove(entity);

    public async Task<int> SaveChangesAsync()
        => await context.SaveChangesAsync();

    public async Task<EntityEntry<T>> AddAsync(T? entity)
        => await context.Set<T?>().AddAsync(entity);
}