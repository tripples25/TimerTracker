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

    public async Task<ActionResult<T>> CreateOrUpdate(T entireEntity)
    {
        var dbEntity = await repository.FindAsync(entireEntity.Id);
        var isCreated = false;

        if (dbEntity is null)
        {
            entireEntity.UpdateFieldsFromEntity();
            isCreated = true; // TODO: можно сократить isCreated = dbEntity is null
            await repository.AddAsync(entireEntity);
        }
        else
        {
            dbEntity.CreateFieldsFromEntity(entireEntity); // TODO: Rename не отражает смысл метода
        }

        await repository.SaveChangesAsync();

        return Ok(new CreateOrUpdateResponse
        {
            Id = entireEntity.Id,
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