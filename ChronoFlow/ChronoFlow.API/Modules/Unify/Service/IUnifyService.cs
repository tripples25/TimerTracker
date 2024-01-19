using System.Linq.Expressions;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Modules;

public interface IUnifyService<T>
{
    Task<ActionResult<IEnumerable<T>>> GetAll();
    Task<ActionResult<T>> Get([FromRoute] Guid id);
    Task<ActionResult<T>> CreateOrUpdate([FromBody] T eventEntity);
    Task<ActionResult> Delete([FromBody] Guid id);
    Task<ActionResult<T>> StopTracking([FromBody] T eventEntity);
}