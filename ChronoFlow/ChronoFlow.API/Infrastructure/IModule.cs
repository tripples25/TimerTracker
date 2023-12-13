namespace ChronoFlow.API.DAL;

public interface IModule
{
    IServiceCollection RegisterModule(IServiceCollection services);
}

public static class ModuleExtensions
{
    private static List<IModule>? modules;
    private static List<IModule> Modules => modules ??= DiscoverModules().ToList();

    public static IServiceCollection RegisterModules(this IServiceCollection services)
    {
        foreach (var module in Modules)
            module.RegisterModule(services);

        return services;
    }

    private static IEnumerable<IModule> DiscoverModules()
    {
        return typeof(IModule).Assembly
            .GetTypes()
            .Where(p => p.IsClass && p.IsAssignableTo(typeof(IModule)))
            .Select(Activator.CreateInstance)
            .Cast<IModule>();
    }
}