namespace Smart.Resolver;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Smart.Resolver.Components;

public sealed class SmartChildResolver : IResolver, IContainer, IDisposableTracker
{
    [ThreadStatic]
    private static ContainerSlot? pool;

    private readonly SmartResolver resolver;

    private readonly ContainerSlot slot;

    private int disposed;

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
        if (Interlocked.CompareExchange(ref disposed, 1, 0) == 1)
        {
            return;
        }

        slot.Clear();
        pool ??= slot;
    }

    void IDisposableTracker.TrackDisposable(IDisposable disposable) => slot.AddDisposable(disposable);

    // CanGet

    public bool CanGet<T>() => resolver.CanGet<T>();

    public bool CanGet<T>(object? key) => resolver.CanGet<T>(key);

    public bool CanGet(Type type) => resolver.CanGet(type);

    public bool CanGet(Type type, object? key) => resolver.CanGet(type, key);

    // TryGet

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<T>([MaybeNullWhen(false)] out T obj)
    {
        var entry = resolver.FindFactoryEntry(this, typeof(T));
        if (entry.CanGet)
        {
            obj = UnsafeCast<T>(entry.Constant ?? entry.Single(this));
            return true;
        }

        obj = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<T>(object? key, [MaybeNullWhen(false)] out T obj)
    {
        var entry = resolver.FindFactoryEntry(this, typeof(T), key);
        if (entry.CanGet)
        {
            obj = UnsafeCast<T>(entry.Constant ?? entry.Single(this));
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
            obj = entry.Constant ?? entry.Single(this);
            return true;
        }

        obj = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet(Type type, object? key, [MaybeNullWhen(false)] out object obj)
    {
        var entry = resolver.FindFactoryEntry(this, type, key);
        if (entry.CanGet)
        {
            obj = entry.Constant ?? entry.Single(this);
            return true;
        }

        obj = default;
        return false;
    }

    // Get

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<T>()
    {
        var entry = resolver.FindFactoryEntry(this, typeof(T));
        return UnsafeCast<T>(entry.Constant ?? entry.Single(this));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<T>(object? key)
    {
        var entry = resolver.FindFactoryEntry(this, typeof(T), key);
        return UnsafeCast<T>(entry.Constant ?? entry.Single(this));
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object Get(Type type)
    {
        var entry = resolver.FindFactoryEntry(this, type);
        return entry.Constant ?? entry.Single(this);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object Get(Type type, object? key)
    {
        var entry = resolver.FindFactoryEntry(this, type, key);
        return entry.Constant ?? entry.Single(this);
    }

    // GetAll

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<T> GetAll<T>() => FactoryEnumerable.Create<T>(this, resolver.FindFactoryEntry(this, typeof(T)).Multiple);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<T> GetAll<T>(object? key) => FactoryEnumerable.Create<T>(this, resolver.FindFactoryEntry(this, typeof(T), key).Multiple);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<object> GetAll(Type type) => FactoryEnumerable.Create<object>(this, resolver.FindFactoryEntry(this, type).Multiple);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<object> GetAll(Type type, object? key) => FactoryEnumerable.Create<object>(this, resolver.FindFactoryEntry(this, type, key).Multiple);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Inject(object instance)
    {
        var actions = resolver.FindInjectors(instance.GetType());
        for (var i = 0; i < actions.Length; i++)
        {
            actions[i](this, instance);
        }
    }

    // Helper

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T UnsafeCast<T>(object obj)
    {
        if (typeof(T).IsValueType)
        {
            return (T)obj;
        }

        ref var r = ref Unsafe.As<object, T>(ref obj);
        return r;
    }

    // IServiceProvider

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object GetService(Type serviceType) => Get(serviceType);
}
