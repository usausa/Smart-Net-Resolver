namespace Smart.Resolver;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Smart.Resolver.Components;

// TODO slot with constraint
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanGet<T>() => resolver.FindFactoryEntry(this, typeof(T)).CanGet;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanGet<T>(object? parameter) => resolver.FindFactoryEntry(this, typeof(T), parameter).CanGet;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanGet(Type type) => resolver.FindFactoryEntry(this, type).CanGet;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanGet(Type type, object? parameter) => resolver.FindFactoryEntry(this, type, parameter).CanGet;

    // TryGet

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<T>(object? parameter, [MaybeNullWhen(false)] out T obj)
    {
        var entry = resolver.FindFactoryEntry(this, typeof(T), parameter);
        if (entry.CanGet)
        {
            obj = (T)entry.Single(this);
            return true;
        }

        obj = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet(Type type, object? parameter, [MaybeNullWhen(false)] out object obj)
    {
        var entry = resolver.FindFactoryEntry(this, type, parameter);
        if (entry.CanGet)
        {
            obj = entry.Single(this);
            return true;
        }

        obj = default;
        return false;
    }

    // Get

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<T>() => (T)resolver.FindFactoryEntry(this, typeof(T)).Single(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<T>(object? parameter) => (T)resolver.FindFactoryEntry(this, typeof(T), parameter).Single(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object Get(Type type) => resolver.FindFactoryEntry(this, type).Single(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object Get(Type type, object? parameter) => resolver.FindFactoryEntry(this, type, parameter).Single(this);

    // GetAll

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<T> GetAll<T>() => resolver.FindFactoryEntry(this, typeof(T)).Multiple.Select(x => (T)x(this));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<T> GetAll<T>(object? parameter) => resolver.FindFactoryEntry(this, typeof(T), parameter).Multiple.Select(x => (T)x(this));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<object> GetAll(Type type) => resolver.FindFactoryEntry(this, type).Multiple.Select(x => x(this));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<object> GetAll(Type type, object? parameter) => resolver.FindFactoryEntry(this, type, parameter).Multiple.Select(x => x(this));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
