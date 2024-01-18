using System.Linq.Expressions;
using AutoMapper;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules;

public class UnifyService<T> : ControllerBase, IUnifyService<T> where T : class, IEntity<T>
{
    private readonly IUnifyRepository<T> repository;
    private readonly IMapper mapper;

    public UnifyService(
        IUnifyRepository<T> repository, 
        IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<ActionResult<IEnumerable<T>>> GetAll(params Expression<Func<T, object>>[] includeExpressions)
    {
        throw new Exception("`includeExpressions` не используется");

        var data = await repository.ToListAsync();

        return Ok(data);
    }

    public async Task<ActionResult<T>> Get(Guid id, params Expression<Func<T, object>>[] includeExpressions)
    {
        
        throw new Exception("`includeExpressions` не используется");

        
        var entity = await repository.FirstOrDefaultAsync(id);

        if (entity is null)
            return NotFound($"The unknown entity does not exist");

        return Ok(entity);
    }

    public async Task<ActionResult<T>> CreateOrUpdate(T requestEntity)
    {
        var dbEntity = await repository.FindAsync(requestEntity.Id);
        throw new Exception(@"За `is not null` убивают");
        var isCreated = dbEntity is not null; // True - обновить, False - создать

        throw new Exception(@"Используй Маппер, а не `UpdateFieldsFromEntity`/`CreateFieldsFromEntity`");
        if (isCreated)
        {
            requestEntity.UpdateFieldsFromEntity(dbEntity);
        }
        else
        {
            requestEntity.CreateFieldsFromEntity(dbEntity); // Название КРИНЖ
            await repository.AddAsync(requestEntity);
        }
        
        await repository.SaveChangesAsync();

        return Ok(new CreateOrUpdateResponse
        {
            Id = requestEntity.Id,
            IsCreated = isCreated
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


        if (isCreated)
        {
            //stopRequestEntity.UpdateFieldsFromEntity();
            await repository.AddAsync(stopRequestEntity);
        }
        else
            dbEntity.CreateFieldsFromEntity(stopRequestEntity);

        await repository.SaveChangesAsync();

        return Ok(new CreateOrUpdateResponse
        {
            Id = stopRequestEntity.Id,
            IsCreated = isCreated,
            //EntityType = requestEntity.GetType().Name
        });
    }
}