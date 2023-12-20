using ChronoFlow.API.DAL;
using ChronoFlow.API.DAL.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.Modules.TemplatesModule;

public class TemplateRepository : IUnifyRepository<TemplateEntity>
{
    private readonly ApplicationDbContext context;

    public TemplateRepository(ApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<List<TemplateEntity>> ToListAsync()
        => await context.Templates.ToListAsync();
    
    public async Task<TemplateEntity?> FirstOrDefaultAsync(Guid id)
        => await context.Templates.FirstOrDefaultAsync(e => e.Id == id);

    public async Task<TemplateEntity?> FindAsync(Guid id)
        => await context.Templates.FindAsync(id);

    public void Remove(TemplateEntity? entity)
        => context.Templates.Remove(entity);

    public async Task<int> SaveChangesAsync()
        => await context.SaveChangesAsync();

    public async Task<EntityEntry<TemplateEntity>> AddAsync(TemplateEntity? entity)
        => await context.Templates.AddAsync(entity);
}