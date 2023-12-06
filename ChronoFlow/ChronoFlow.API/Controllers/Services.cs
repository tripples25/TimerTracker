using ChronoFlow.API.DAL;
using ChronoFlow.API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace ChronoFlow.API.Controllers;

[ApiController]
[Route("[controller]")]

public class Services : ControllerBase
{
    private readonly ApplicationDbContext context;

    public Services(ApplicationDbContext context)
    {
        this.context = context;
    }
    
    [HttpGet("getEventTemplate")]
    public async Task<IActionResult> GetTemplate()
    {
        var data = await context.EventTemplates.ToListAsync();
        
        return Ok(data);
    }

    [HttpPost("createEventTemplate")]
    public async Task<EventTemplate> CreateTemplate([FromQuery] string template)
    {
        return new EventTemplate();
    }
    
    [HttpPatch("updateEventTemplate")]
    public async Task<EventTemplate> UpdateEvent([FromQuery] string template)
    {
        return new EventTemplate();
    }

    [HttpGet("getUserInfo")]
    public async Task<IActionResult> GetUser([FromQuery] string username)
    {
        var data = await context.Users.ToListAsync();
        
        return Ok(data);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] User user)
    {
        var newUser = new User { Id = user.Id, Nickname = user.Nickname };
        
        //var result = await

        return Ok();
    }
    
    
    
    
    
}