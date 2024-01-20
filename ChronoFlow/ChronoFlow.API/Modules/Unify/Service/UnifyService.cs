using AutoMapper;
using ChronoFlow.API.DAL.Entities;
using ChronoFlow.API.DAL.Entities.Response;
using log4net;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules;

public class UnifyService<T> : ControllerBase, IUnifyService<T> where T : class, IEntity<T>
{
    private readonly IUnifyRepository<T> repository;
    private readonly IMapper mapper;
    private static readonly ILog log = LogManager.GetLogger(typeof(UnifyService<T>));

    public UnifyService(IUnifyRepository<T> repository, IMapper mapper)
    {
        this.repository = repository;
        this.mapper = mapper;
    }

    public async Task<ActionResult<IEnumerable<T>>> GetAll()
    {
        var data = await repository.ToListAsync();
        log.Info("GET request for all entites received");
        return Ok(data);
    }

    public async Task<ActionResult<T>> Get(Guid id)
    {
        var entity = await repository.FirstOrDefaultAsync(id);

        if (entity is null)
        {
            log.Info("GET request for specific entity: the entity was not found");
            return NotFound("The unknown entity does not exist");
        }

        log.Info("GET request for all entites received");
        return Ok(entity);
    }

    public async Task<ActionResult<T>> CreateOrUpdate(T requestEntity)
    {
        var dbEntity = await repository.FindAsync(requestEntity.Id);
        var isCreated = dbEntity != null; // True - обновить, False - создать

        if (isCreated)
        {
            try
            {
                mapper.Map(requestEntity, dbEntity);
            }

            catch (Exception ex)
            {
                log.Error($"An error occurred while mapping the entity: {requestEntity.GetType().Name}", ex);
                throw;
            }

            log.Info($"The entity was successfully added: {requestEntity.GetType().Name}");
        }

        else
        {
            try
            {
                await repository.AddAsync(requestEntity);
            }

            catch (Exception ex)
            {
                log.Error($"An error occurred while creating the entity: {requestEntity.GetType().Name}", ex);
                throw;
            }

            log.Info($"The entity was successfully created: {requestEntity.GetType().Name}");
        }

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
            log.Info($"The entity was successfully deleted: {dbEntity.GetType().Name}");
            await repository.SaveChangesAsync();
        }
        else
            log.Info("The entity was not found while deletion");

        return NoContent();
    }
}