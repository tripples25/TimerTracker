using ChronoFlow.API.DAL.Entities;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace ChronoFlow.API.Modules.TemplatesModule;

public interface ITemplateRepository
{
    public Task<List<TemplateEntity>> ToListAsync();
    public Task<TemplateEntity?> FirstOrDefaultAsync(Guid id);
    public Task<TemplateEntity?> FindAsync(Guid id);
    public void Remove(TemplateEntity? entity);
    public Task<int> SaveChangesAsync();
    public Task<EntityEntry<TemplateEntity>> AddAsync(TemplateEntity? entity);
}