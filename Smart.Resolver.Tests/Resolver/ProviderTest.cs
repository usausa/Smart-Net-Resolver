namespace Smart.Resolver;

using Smart.Collections.Generic;
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
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("foo");
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("bar");
        config.Bind<ArrayInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<ArrayInjectedObject>();
        var foo = resolver.Get<SimpleObject>("foo");
        var bar = resolver.Get<SimpleObject>("bar");

        Assert.Equal(2, obj.Objects.Length);
        Assert.NotSame(foo, bar);
        Assert.True(obj.Objects.Contains(foo, static x => x));
        Assert.True(obj.Objects.Contains(bar, static x => x));
    }

    [Fact]
    public void ObjectEnumerableCreatedByStandardProvider()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("foo");
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("bar");
        config.Bind<EnumerableInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<EnumerableInjectedObject>();
        var foo = resolver.Get<SimpleObject>("foo");
        var bar = resolver.Get<SimpleObject>("bar");

        Assert.Equal(2, obj.Objects.Count());
        Assert.NotSame(foo, bar);
        Assert.True(obj.Objects.Contains(foo, static x => x));
        Assert.True(obj.Objects.Contains(bar, static x => x));
    }

    [Fact]
    public void ObjectCollectionCreatedByStandardProvider()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("foo");
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("bar");
        config.Bind<CollectionInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<CollectionInjectedObject>();
        var foo = resolver.Get<SimpleObject>("foo");
        var bar = resolver.Get<SimpleObject>("bar");

        Assert.Equal(2, obj.Objects.Count);
        Assert.NotSame(foo, bar);
        Assert.True(obj.Objects.Contains(foo, static x => x));
        Assert.True(obj.Objects.Contains(bar, static x => x));
    }

    [Fact]
    public void ObjectListCreatedByStandardProvider()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("foo");
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("bar");
        config.Bind<ListInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<ListInjectedObject>();
        var foo = resolver.Get<SimpleObject>("foo");
        var bar = resolver.Get<SimpleObject>("bar");

        Assert.Equal(2, obj.Objects.Count);
        Assert.NotSame(foo, bar);
        Assert.True(obj.Objects.Contains(foo, static x => x));
        Assert.True(obj.Objects.Contains(bar, static x => x));
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
        public SimpleObject[] Objects { get; }

        public ArrayInjectedObject(SimpleObject[] objects)
        {
            Objects = objects;
        }
    }
#pragma warning restore CA1819

    public sealed class EnumerableInjectedObject
    {
        public IEnumerable<SimpleObject> Objects { get; }

        public EnumerableInjectedObject(IEnumerable<SimpleObject> objects)
        {
            Objects = objects;
        }
    }

    public sealed class CollectionInjectedObject
    {
        public ICollection<SimpleObject> Objects { get; }

        public CollectionInjectedObject(ICollection<SimpleObject> objects)
        {
            Objects = objects;
        }
    }

    public sealed class ListInjectedObject
    {
        public IList<SimpleObject> Objects { get; }

        public ListInjectedObject(IList<SimpleObject> objects)
        {
            Objects = objects;
        }
    }
}
