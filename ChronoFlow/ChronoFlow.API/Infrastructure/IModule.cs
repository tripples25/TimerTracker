namespace ChronoFlow.API.DAL;

public interface IModule
{
    public IServiceCollection RegisterModule(IServiceCollection services);
}