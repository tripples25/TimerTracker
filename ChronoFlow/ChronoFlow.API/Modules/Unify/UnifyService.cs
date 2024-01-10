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

    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        var data = await repository.ToListAsync();

        return Ok(data);
    }

    public async Task<ActionResult<T>> Get(Guid id)
    {
        var entity = await repository.FirstOrDefaultAsync(id);

        if (entity is null)
            return NotFound("The template does not exist");

        return Ok(entity);
    }

    public async Task<ActionResult<T>> CreateOrUpdate(T requestEntity)
    {
        var dbEntity = await repository.FindAsync(requestEntity.Id);
        var isCreated = false;

        if (dbEntity is null)
        {
            requestEntity.UpdateFieldsFromEntity();
            isCreated = true; // TODO: можно сократить isCreated = dbEntity is null
            await repository.AddAsync(requestEntity);
        }
        else
        {
            dbEntity.CreateFieldsFromEntity(requestEntity); // TODO: Rename не отражает смысл метода
        }

        await repository.SaveChangesAsync();

        return Ok(new CreateOrUpdateResponse
        {
            Id = requestEntity.Id,
            IsCreated = isCreated,
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