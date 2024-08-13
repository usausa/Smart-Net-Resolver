namespace WorkKeyed;

using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

using Smart.Resolver;

public static class Program
{
    public static void Main()
    {
        // TODO 1. Populate
        // TODO 2. IConstraint変更 改造
        // TODO 3. BindSingleton等の拡張
        // TODO 4. Benchmark re
        var services = new ServiceCollection();
        services.AddKeyedSingleton<IService, Service1>(nameof(Service1));
        services.AddKeyedSingleton<IService, Service2>(nameof(Service2));

        var serviceProviderFactory = new SmartServiceProviderFactory();
        var config = serviceProviderFactory.CreateBuilder(services);

        //config.Bind<IService>().To<Service1>().InSingletonScope().Named(nameof(Service1));
        //config.Bind<IService>().To<Service2>().InSingletonScope().Named(nameof(Service2));

        var resolver = serviceProviderFactory.CreateServiceProvider(config);

        var service1 = resolver.GetRequiredKeyedService<IService>(nameof(Service1));
        var service2 = resolver.GetRequiredKeyedService<IService>(nameof(Service2));

        service1.Execute();
        service2.Execute();
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