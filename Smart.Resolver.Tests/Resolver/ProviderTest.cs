namespace Smart.Resolver;

using System.Linq;

using Smart.Resolver.Mocks;

public sealed class ProviderTest
{
    [Fact]
    public void ObjectCreatedByConstantProvider()
    {
        var config = new ResolverConfig();
        var service = new Service();
        config.Bind<IService>().ToConstant(service);
        config.Bind<Controller>().ToSelf();

        using var resolver = config.ToResolver();
        var controller = resolver.Get<Controller>();

        Assert.Same(service, controller.Service);
    }

    [Fact]
    public void ObjectCreatedByCallbackProvider()
    {
        var config = new ResolverConfig();
        var service = new Service();
        config.Bind<IService>().ToMethod(_ => service);
        config.Bind<Controller>().ToSelf();

        using var resolver = config.ToResolver();
        var controller = resolver.Get<Controller>();

        Assert.Same(service, controller.Service);
    }

    [Fact]
    public void ObjectArrayCreatedByStandardProvider()
    {
        var config = new ResolverConfig();
        config.Bind<IService>().To<Service1>().InSingletonScope();
        config.Bind<IService>().To<Service2>().InSingletonScope();
        config.Bind<ArrayInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<ArrayInjectedObject>();
        var services = resolver.GetAll<IService>().ToList();

        Assert.Equal(2, obj.Services.Length);
        Assert.True(obj.Services.All(services.Contains));
    }

    [Fact]
    public void ObjectEnumerableCreatedByStandardProvider()
    {
        var config = new ResolverConfig();
        config.Bind<IService>().To<Service1>().InSingletonScope();
        config.Bind<IService>().To<Service2>().InSingletonScope();
        config.Bind<EnumerableInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<EnumerableInjectedObject>();
        var services = resolver.GetAll<IService>().ToList();

        Assert.Equal(2, obj.Services.Count());
        Assert.True(obj.Services.All(services.Contains));
    }

    [Fact]
    public void ObjectCollectionCreatedByStandardProvider()
    {
        var config = new ResolverConfig();
        config.Bind<IService>().To<Service1>().InSingletonScope();
        config.Bind<IService>().To<Service2>().InSingletonScope();
        config.Bind<CollectionInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<CollectionInjectedObject>();
        var services = resolver.GetAll<IService>().ToList();

        Assert.Equal(2, obj.Services.Count);
        Assert.True(obj.Services.All(services.Contains));
    }

    [Fact]
    public void ObjectListCreatedByStandardProvider()
    {
        var config = new ResolverConfig();
        config.Bind<IService>().To<Service1>().InSingletonScope();
        config.Bind<IService>().To<Service2>().InSingletonScope();
        config.Bind<ListInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<ListInjectedObject>();
        var services = resolver.GetAll<IService>().ToList();

        Assert.Equal(2, obj.Services.Count);
        Assert.True(obj.Services.All(services.Contains));
    }

    [Fact]
    public void ObjectIsCreatedByMaxParameterConstructor()
    {
        var config = new ResolverConfig().UseAutoBinding();
        config.Bind<IService>().To<Service>().InSingletonScope();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<MultiConstructorObject>();

        Assert.Equal(2, obj.Arguments);
    }

    [Fact]
    public void ObjectIsCreatedByBestConstructor()
    {
        var config = new ResolverConfig().UseAutoBinding();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<MultiConstructorObject>();

        Assert.Equal(1, obj.Arguments);
    }

    public sealed class MultiConstructorObject
    {
        public int Arguments { get; }

        public SimpleObject SimpleObject { get; }

        public IService? Service { get; }

        public MultiConstructorObject(SimpleObject simpleObject)
        {
            Arguments = 1;
            SimpleObject = simpleObject;
        }

        public MultiConstructorObject(SimpleObject simpleObject, IService service)
        {
            Arguments = 2;
            SimpleObject = simpleObject;
            Service = service;
        }
    }

#pragma warning disable CA1819
    public sealed class ArrayInjectedObject
    {
        public IService[] Services { get; }

        public ArrayInjectedObject(IService[] services)
        {
            Services = services;
        }
    }
#pragma warning restore CA1819

    public sealed class EnumerableInjectedObject
    {
        public IEnumerable<IService> Services { get; }

        public EnumerableInjectedObject(IEnumerable<IService> services)
        {
            Services = services;
        }
    }

    public sealed class CollectionInjectedObject
    {
        public ICollection<IService> Services { get; }

        public CollectionInjectedObject(ICollection<IService> services)
        {
            Services = services;
        }
    }

    public sealed class ListInjectedObject
    {
        public IList<IService> Services { get; }

        public ListInjectedObject(IList<IService> services)
        {
            Services = services;
        }
    }
}
