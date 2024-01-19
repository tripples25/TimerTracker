using System.Linq.Expressions;
using AutoMapper;
using Catalyst.Models;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using ChronoFlow.API.Modules.EventsModule;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules;

public class UnifyService<T> : ControllerBase, IUnifyService<T> where T : class, IEntity<T>
{
    private readonly IUnifyRepository<T> repository;
    private readonly IMapper mapper;

    public UnifyService(IUnifyRepository<T> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
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
            return NotFound($"The unknown entity does not exist");

        return Ok(entity);
    }

    public async Task<ActionResult<T>> CreateOrUpdate(T requestEntity)
    {
        var dbEntity = await repository.FindAsync(requestEntity.Id);
        var isCreated = dbEntity != null; // True - обновить, False - создать
        
        if (isCreated)
            mapper.Map(requestEntity, dbEntity);
        else
            await repository.AddAsync(requestEntity);


        await repository.SaveChangesAsync();

        return Ok(new CreateOrUpdateResponse
        {
            Id = requestEntity.Id,
            IsCreated = isCreated
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