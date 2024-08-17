namespace WorkKeyed;

using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

using Smart.Resolver;

public static class Program
{
    public static void Main()
    {
        var services = new ServiceCollection();
        services.AddKeyedSingleton<IService, Service1>(nameof(Service1));
        services.AddKeyedSingleton<IService, Service2>(nameof(Service2));
        services.AddSingleton<Service1Reference>();

        var serviceProviderFactory = new SmartServiceProviderFactory();

        // Create builder
        var config = serviceProviderFactory.CreateBuilder(services);

        //config.Bind<IService>().To<Service1>().InSingletonScope().Named(nameof(Service1));
        //config.Bind<IService>().To<Service2>().InSingletonScope().Named(nameof(Service2));

        // Create service provider
        var resolver = serviceProviderFactory.CreateServiceProvider(config);

        // Use
        var service1 = resolver.GetRequiredKeyedService<IService>(nameof(Service1));
        var service2 = resolver.GetRequiredKeyedService<IService>(nameof(Service2));
        service1.Execute();
        service2.Execute();

        var service1Reference = resolver.GetRequiredService<Service1Reference>();
        service1Reference.Service.Execute();
    }
}

public interface IService
{
    void Execute();
}

public class Service1 : IService
{
    public void Execute()
    {
        Debug.WriteLine("Service1");
    }
}

public class Service2 : IService
{
    public void Execute()
    {
        Debug.WriteLine("Service2");
    }
}

public class Service1Reference
{
    public IService Service { get; }

    public Service1Reference([FromKeyedServices("Service1")] IService service)
    {
        Service = service;
    }
}
