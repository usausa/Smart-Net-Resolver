namespace Smart.Resolver;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Smart.Resolver.Components;
using Smart.Resolver.Constraints;

public sealed class SmartChildResolver : IResolver, IContainer
{
    [ThreadStatic]
    private static ContainerSlot? pool;

    private readonly SmartResolver resolver;

    private readonly ContainerSlot slot;

    ContainerSlot IContainer.Slot => slot;

    public SmartChildResolver(SmartResolver resolver)
    {
        this.resolver = resolver;

        if (pool is null)
        {
            slot = new ContainerSlot();
        }
        else
        {
            slot = pool;
            pool = null;
        }
    }

    public void Dispose()
    {
        slot.Clear();
        pool ??= slot;
    }

    // CanGet

    public bool CanGet<T>() => resolver.FindFactoryEntry(this, typeof(T)).CanGet;

    public bool CanGet<T>(IConstraint constraint) => resolver.FindFactoryEntry(this, typeof(T), constraint).CanGet;

    public bool CanGet(Type type) => resolver.FindFactoryEntry(this, type).CanGet;

    public bool CanGet(Type type, IConstraint constraint) => resolver.FindFactoryEntry(this, type, constraint).CanGet;

    // TryGet

    public bool TryGet<T>([MaybeNullWhen(false)] out T obj)
    {
        var entry = resolver.FindFactoryEntry(this, typeof(T));
        if (entry.CanGet)
        {
            obj = (T)entry.Single(this);
            return true;
        }

        obj = default;
        return false;
    }

    public bool TryGet<T>(IConstraint constraint, [MaybeNullWhen(false)] out T obj)
    {
        var entry = resolver.FindFactoryEntry(this, typeof(T), constraint);
        if (entry.CanGet)
        {
            obj = (T)entry.Single(this);
            return true;
        }

        obj = default;
        return false;
    }

    public bool TryGet(Type type, [MaybeNullWhen(false)] out object obj)
    {
        var entry = resolver.FindFactoryEntry(this, type);
        if (entry.CanGet)
        {
            obj = entry.Single(this);
            return true;
        }

        obj = default;
        return false;
    }

    public bool TryGet(Type type, IConstraint constraint, [MaybeNullWhen(false)] out object obj)
    {
        var entry = resolver.FindFactoryEntry(this, type, constraint);
        if (entry.CanGet)
        {
            obj = entry.Single(this);
            return true;
        }

        obj = default;
        return false;
    }

    // Get

    public T Get<T>() => (T)resolver.FindFactoryEntry(this, typeof(T)).Single(this);

    public T Get<T>(IConstraint constraint) => (T)resolver.FindFactoryEntry(this, typeof(T), constraint).Single(this);

    public object Get(Type type) => resolver.FindFactoryEntry(this, type).Single(this);

    public object Get(Type type, IConstraint constraint) => resolver.FindFactoryEntry(this, type, constraint).Single(this);

    // GetAll

    public IEnumerable<T> GetAll<T>() => resolver.FindFactoryEntry(this, typeof(T)).Multiple.Select(x => (T)x(this));

    public IEnumerable<T> GetAll<T>(IConstraint constraint) => resolver.FindFactoryEntry(this, typeof(T), constraint).Multiple.Select(x => (T)x(this));

    public IEnumerable<object> GetAll(Type type) => resolver.FindFactoryEntry(this, type).Multiple.Select(x => x(this));

    public IEnumerable<object> GetAll(Type type, IConstraint constraint) => resolver.FindFactoryEntry(this, type, constraint).Multiple.Select(x => x(this));

    public void Inject(object instance)
    {
        var actions = resolver.FindInjectors(instance.GetType());
        for (var i = 0; i < actions.Length; i++)
        {
            actions[i](this, instance);
        }
    }

    // IServiceProvider

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object GetService(Type serviceType) => Get(serviceType);
}
