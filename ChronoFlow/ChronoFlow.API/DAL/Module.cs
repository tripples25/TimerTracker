using ChronoFlow.API.Infra;
using Microsoft.EntityFrameworkCore;

namespace ChronoFlow.API.DAL;

public class Module : IModule
{
    private readonly IConfiguration configuration;

    public Module(IConfiguration configuration)
    {
        this.configuration = configuration;
    }
    
    public void RegisterModules(IServiceCollection services)
    {
        services.AddSingleton(configuration);
        
        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("TimeTrackerDB")));
        services.AddSingleton(new Config(true));
    }
}