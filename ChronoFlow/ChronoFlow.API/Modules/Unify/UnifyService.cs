using System.Linq.Expressions;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules;

public class UnifyService<T> : ControllerBase, IUnifyService<T> where T : class, IEntity<T>
{
    private readonly IUnifyRepository<T> repository;

    public UnifyService(IUnifyRepository<T> repository)
    {
        this.repository = repository;
    }

    public async Task<ActionResult<IEnumerable<T>>> GetAll(params Expression<Func<T, object>>[] includeExpressions)
    {
        var data = await repository.ToListAsync();

        return Ok(data);
    }

    public async Task<ActionResult<T>> Get(Guid id, params Expression<Func<T, object>>[] includeExpressions)
    {
        var entity = await repository.FirstOrDefaultAsync(id);

        if (entity is null)
            return NotFound($"The unknown entity does not exist");

        return Ok(entity);
    }

    public async Task<ActionResult<T>> CreateOrUpdate(T requestEntity)
    {
        var dbEntity = await repository.FindAsync(requestEntity.Id);
        var isCreated = dbEntity is not null;

        if (isCreated)
        {
            requestEntity.UpdateFieldsFromEntity();
            await repository.AddAsync(requestEntity);
        }
        else
            dbEntity.CreateFieldsFromEntity(requestEntity);

        await repository.SaveChangesAsync();

        return Ok(new CreateOrUpdateResponse
        {
            Id = requestEntity.Id,
            IsCreated = isCreated,
            //EntityType = requestEntity.GetType().Name
        });
    }

    public async Task<ActionResult> Delete(Guid id)
    {
        var dbEntity = await repository.FindAsync(id);
        
        if (dbEntity != null)
        {
            repository.Remove(dbEntity);
            await repository.SaveChangesAsync();
        }

        return NoContent();
    }


}