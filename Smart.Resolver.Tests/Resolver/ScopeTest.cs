namespace Smart.Resolver;

using Smart.ComponentModel;
using Smart.Resolver.Mocks;
using Smart.Resolver.Scopes;

using Xunit;

public class ScopeTest
{
    [Fact]
    public void ObjectInTransientScopeAreNotSame()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InTransientScope();

        using var resolver = config.ToResolver();
        var obj1 = resolver.Get<SimpleObject>();
        var obj2 = resolver.Get<SimpleObject>();

        Assert.NotSame(obj1, obj2);
    }

    [Fact]
    public void ObjectInSingletonScopeAreSame()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope();

        using var resolver = config.ToResolver();
        var obj1 = resolver.Get<SimpleObject>();
        var obj2 = resolver.Get<SimpleObject>();

        Assert.Same(obj1, obj2);
    }

    [Fact]
    public void ObjectInContainerScopeOfSameChildAreSame()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InContainerScope();

        using var resolver = config.ToResolver();
        using var child = resolver.CreateChildResolver();
        var obj1 = child.Get<SimpleObject>();
        var obj2 = child.Get<SimpleObject>();

        Assert.Same(obj1, obj2);
    }

    [Fact]
    public void ObjectInContainerScopeOfDifferentChildAreDifferent()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InContainerScope();

        using var resolver = config.ToResolver();
        using var child1 = resolver.CreateChildResolver();
        using var child2 = resolver.CreateChildResolver();
        var obj1 = child1.Get<SimpleObject>();
        var obj2 = child2.Get<SimpleObject>();

        Assert.NotSame(obj1, obj2);
    }

    [Fact]
    public void ObjectInContainerScopeOfParentAreDifferent()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InContainerScope();

        using var resolver = config.ToResolver();
        var obj1 = resolver.Get<SimpleObject>();
        var obj2 = resolver.Get<SimpleObject>();

        Assert.NotSame(obj1, obj2);
    }

    [Fact]
    public void ObjectInCustomScope()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InScope(_ => new CustomScope());

        using var resolver = config.ToResolver();
        var obj1 = resolver.Get<SimpleObject>();
        var obj2 = resolver.Get<SimpleObject>();

        Assert.Same(obj1, obj2);
    }

    [Fact]
    public void ObjectInSingletonScopeAreDisposedWhenResolverDisposed()
    {
        var config = new ResolverConfig();
        config.Bind<DisposeTrackingObject>().ToSelf().InSingletonScope();

        DisposeTrackingObject obj;
        using (var resolver = config.ToResolver())
        {
            obj = resolver.Get<DisposeTrackingObject>();
        }

        Assert.True(obj.Disposed);
    }

    public sealed class DisposeTrackingObject : IDisposable
    {
        public bool Disposed { get; private set; }

        public void Dispose()
        {
            Disposed = true;
        }
    }

    public sealed class CustomScope : IScope
    {
        private static readonly ThreadLocal<Dictionary<CustomScope, object>> Cache =
            new(() => new Dictionary<CustomScope, object>());

        public IScope Copy(ComponentContainer components)
        {
            return this;
        }

        public Func<IResolver, object> Create(Func<object> factory)
        {
            return _ =>
            {
                if (Cache.Value!.TryGetValue(this, out var value))
                {
                    return value;
                }

                value = factory();
                Cache.Value![this] = value;

                return value;
            };
        }
    }
}
