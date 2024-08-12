namespace WorkKeyed;

using System.Diagnostics;

using Microsoft.Extensions.DependencyInjection;

using Smart.Resolver;

public static class Program
{
    public static void Main()
    {
        // TODO 4. BindSingleton等の拡張
        // TODO 4. IConstraint変更
        // TODO 3. Populate
        //services.AddKeyedSingleton<IService, Service1>(nameof(Service1));
        //services.AddKeyedSingleton<IService, Service2>(nameof(Service2));
        // TODO 1. test basic
        var services = new ServiceCollection();
        //services.AddSingleton<IService, Service1>();

        var serviceProviderFactory = new SmartServiceProviderFactory();
        var config = serviceProviderFactory.CreateBuilder(services);

        config.Bind<IService>().To<Service1>().InSingletonScope().Named(nameof(Service1));
        config.Bind<IService>().To<Service2>().InSingletonScope().Named(nameof(Service2));

        var resolver = serviceProviderFactory.CreateServiceProvider(config);

        var service = resolver.GetRequiredService<IService>();

        service.Execute();

        // TODO 2. Implement keyed

        //var provider = services.BuildServiceProvider();

        //var service1 = provider.GetRequiredKeyedService<IService>(nameof(Service1));
        //var service2 = provider.GetRequiredKeyedService<IService>(nameof(Service2));

        //service1.Execute();
        //service2.Execute();

        // IKeyedServiceProviderを実装しているか
        // public sealed class ServiceProvider : IServiceProvider, IKeyedServiceProvider, IDisposable, IAsyncDisposable
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
