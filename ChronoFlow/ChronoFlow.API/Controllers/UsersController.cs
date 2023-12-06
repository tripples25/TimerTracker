using ChronoFlow.API.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ChronoFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly ApplicationDbContext context;

    public UsersController(ApplicationDbContext context)
    {
        this.context = context;
    }
    
    [HttpGet("{userId:string}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid userId)
    {
        var data = await context.Users.FindAsync(userId);
        
        return Ok(data);
    }
}