namespace Smart.Resolver;

using Smart.ComponentModel;
using Smart.Resolver.Scopes;

#pragma warning disable CA1034
public sealed class DisposalTrackingTest
{
    [Fact]
    public void TransientDisposedInReverseOrderOnResolverDispose()
    {
        var config = new ResolverConfig();
        config.UseOption(new ResolverOption { DisposalTracking = true });
        config.Bind<DisposeCallback>().ToSelf().InSingletonScope();
        config.Bind<TrackedObject>().ToSelf().InTransientScope();

        var resolver = config.ToResolver();
        var callback = resolver.Get<DisposeCallback>();
        var obj1 = resolver.Get<TrackedObject>();
        var obj2 = resolver.Get<TrackedObject>();

        resolver.Dispose();

        Assert.Equal([obj2, obj1], callback.Disposed);
    }

    [Fact]
    public void SingletonAndTransientDisposedInReverseCreationOrder()
    {
        var config = new ResolverConfig();
        config.UseOption(new ResolverOption { DisposalTracking = true });
        config.Bind<DisposeCallback>().ToSelf().InSingletonScope();
        config.Bind<TrackedSingleton>().ToSelf().InSingletonScope();
        config.Bind<TrackedObject>().ToSelf().InTransientScope();

        var resolver = config.ToResolver();
        var callback = resolver.Get<DisposeCallback>();
        var singleton = resolver.Get<TrackedSingleton>();
        var transient = resolver.Get<TrackedObject>();
        resolver.Get<TrackedSingleton>();

        resolver.Dispose();

        Assert.Equal([transient, singleton], callback.Disposed);
    }

    [Fact]
    public void TransientDisposedByOwnerScope()
    {
        var config = new ResolverConfig();
        config.UseOption(new ResolverOption { DisposalTracking = true });
        config.Bind<DisposeCallback>().ToSelf().InSingletonScope();
        config.Bind<TrackedObject>().ToSelf().InTransientScope();

        var resolver = config.ToResolver();
        var callback = resolver.Get<DisposeCallback>();
        var rootObject = resolver.Get<TrackedObject>();

        var child = resolver.CreateChildResolver();
        var scoped1 = child.Get<TrackedObject>();
        var scoped2 = child.Get<TrackedObject>();
        child.Dispose();

        Assert.Equal([scoped2, scoped1], callback.Disposed);

        resolver.Dispose();

        Assert.Equal([scoped2, scoped1, rootObject], callback.Disposed);
    }

    [Fact]
    public void CallbackFactoryDisposableTracked()
    {
        var config = new ResolverConfig();
        config.UseOption(new ResolverOption { DisposalTracking = true });
        config.Bind<DisposeCallback>().ToSelf().InSingletonScope();
        config.Bind<TrackedObject>().ToMethod(static r => new TrackedObject(r.Get<DisposeCallback>())).InTransientScope();

        var resolver = config.ToResolver();
        var callback = resolver.Get<DisposeCallback>();
        var obj = resolver.Get<TrackedObject>();

        resolver.Dispose();

        Assert.Equal([obj], callback.Disposed);
    }

    [Fact]
    public void ConstantNotTracked()
    {
        var callback = new DisposeCallback();
        using var constant = new TrackedObject(callback);

        var config = new ResolverConfig();
        config.UseOption(new ResolverOption { DisposalTracking = true });
        config.Bind<TrackedObject>().ToConstant(constant);

        var resolver = config.ToResolver();
        resolver.Get<TrackedObject>();
        resolver.Dispose();

        Assert.Empty(callback.Disposed);
    }

    [Fact]
    public void TransientNotTrackedWithoutOption()
    {
        var config = new ResolverConfig();
        config.Bind<DisposeCallback>().ToSelf().InSingletonScope();
        config.Bind<TrackedObject>().ToSelf().InTransientScope();

        var resolver = config.ToResolver();
        var callback = resolver.Get<DisposeCallback>();
        resolver.Get<TrackedObject>();

        resolver.Dispose();

        Assert.Empty(callback.Disposed);
    }

    [Fact]
    public void CustomScopeKeepsOwnershipAndIsNotDoubleDisposed()
    {
        var config = new ResolverConfig();
        config.UseOption(new ResolverOption { DisposalTracking = true });
        config.Bind<DisposeCallback>().ToSelf().InSingletonScope();
        config.Bind<TrackedObject>().ToSelf().InScope(static _ => new CachingScope());

        var resolver = config.ToResolver();
        var callback = resolver.Get<DisposeCallback>();
        var obj1 = resolver.Get<TrackedObject>();
        var obj2 = resolver.Get<TrackedObject>();

        Assert.Same(obj1, obj2);

        resolver.Dispose();

        Assert.Empty(callback.Disposed);
    }

    public sealed class CachingScope : IScope
    {
        private object? value;

        public bool TransferDisposal() => false;

        public IScope Copy(ComponentContainer components) => this;

        public Func<IResolver, object> Create(IResolver resolver, Func<IResolver, object> factory)
        {
            return r => value ??= factory(r);
        }
    }

    [Fact]
    public void CustomScopeCanTransferDisposalLikeBuiltInScopes()
    {
        var config = new ResolverConfig();
        config.UseOption(new ResolverOption { DisposalTracking = true });
        config.Bind<DisposeCallback>().ToSelf().InSingletonScope();
        config.Bind<TrackedSingleton>().ToSelf().InSingletonScope();
        config.Bind<TrackedObject>().ToSelf().InScope(static _ => new TransferringScope());

        var resolver = config.ToResolver();
        var callback = resolver.Get<DisposeCallback>();
        var singleton = resolver.Get<TrackedSingleton>();
        var scoped = resolver.Get<TrackedObject>();
        resolver.Get<TrackedObject>();

        resolver.Dispose();

        Assert.Equal([scoped, singleton], callback.Disposed);
    }

    public sealed class TransferringScope : IScope
    {
        private object? value;

        public bool TransferDisposal() => true;

        public IScope Copy(ComponentContainer components) => this;

        public Func<IResolver, object> Create(IResolver resolver, Func<IResolver, object> factory)
        {
            return r => value ??= factory(r);
        }
    }

    public sealed class DisposeCallback
    {
        private readonly List<object> disposed = [];

        public IReadOnlyList<object> Disposed => disposed;

        public void Add(object instance)
        {
            disposed.Add(instance);
        }
    }

    public sealed class TrackedObject : IDisposable
    {
        private readonly DisposeCallback callback;

        public TrackedObject(DisposeCallback callback)
        {
            this.callback = callback;
        }

        public void Dispose()
        {
            callback.Add(this);
        }
    }

    public sealed class TrackedSingleton : IDisposable
    {
        private readonly DisposeCallback callback;

        public TrackedSingleton(DisposeCallback callback)
        {
            this.callback = callback;
        }

        public void Dispose()
        {
            callback.Add(this);
        }
    }
}
