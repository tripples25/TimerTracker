namespace ChronoFlow.API.Infrastructure;

public interface IModule
{
    public void RegisterModule(IServiceCollection services);
}