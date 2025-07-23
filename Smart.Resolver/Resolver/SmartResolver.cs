namespace Smart.Resolver;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

using Smart.Collections.Concurrent;
using Smart.ComponentModel;
using Smart.Linq;
using Smart.Resolver.Bindings;
using Smart.Resolver.Handlers;
using Smart.Resolver.Injectors;
using Smart.Resolver.Providers;

public sealed class SmartResolver : IResolver, IKernel
{
    private readonly ThreadsafeTypeHashArrayMap<FactoryEntry> factoriesCache = new(128);

    private readonly TypeConstraintHashArray<FactoryEntry> factoriesCacheWithConstraint = new();

    private readonly ThreadsafeTypeHashArrayMap<Action<IResolver, object>[]> injectorsCache = new();

    private readonly Func<IResolver, object> nullFactory = static _ => default!;

    private readonly object sync = new();

    private readonly BindingTable table;

    private readonly IInjector[] injectors;

    private readonly IMissingHandler[] handlers;

    private int disposed;

    public ComponentContainer Components { get; }

    public SmartResolver(IResolverConfig config)
    {
        Components = config.CreateComponentContainer();

        injectors = Components.GetAll<IInjector>().ToArray();
        handlers = Components.GetAll<IMissingHandler>().ToArray();

        var tableEntries = new Dictionary<Type, Binding[]>();

        foreach (var group in config.CreateBindings(Components).GroupBy(static b => b.Type))
        {
            tableEntries[group.Key] = [.. group];
        }

        tableEntries[typeof(IResolver)] = [new Binding(typeof(IResolver), new ConstantProvider<IResolver>(this), null, null, null, null, null)];
        tableEntries[typeof(SmartResolver)] = [new Binding(typeof(SmartResolver), new ConstantProvider<SmartResolver>(this), null, null, null, null, null)];
        tableEntries[typeof(IServiceProvider)] = [new Binding(typeof(IServiceProvider), new ConstantProvider<IServiceProvider>(this), null, null, null, null, null)];

        table = new BindingTable(tableEntries);
    }

    public void Dispose()
    {
        if (Interlocked.CompareExchange(ref disposed, 1, 0) == 1)
        {
            return;
        }

        Components.Dispose();
    }

    // ------------------------------------------------------------
    // IServiceProvider
    // ------------------------------------------------------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object GetService(Type serviceType) => Get(serviceType);

    // ------------------------------------------------------------
    // ObjectFactory
    // ------------------------------------------------------------

    bool IKernel.TryResolveFactory(Type type, object? key, out Func<IResolver, object> factory)
    {
        var entry = key is null ? FindFactoryEntry(type) : FindFactoryEntry(type, key);
        factory = entry.Single;
        return entry.CanGet;
    }

    bool IKernel.TryResolveFactories(Type type, object? key, out Func<IResolver, object>[] factories)
    {
        var entry = key is null ? FindFactoryEntry(type) : FindFactoryEntry(type, key);
        factories = entry.Multiple;
        return entry.CanGet;
    }

    // ------------------------------------------------------------
    // Resolver
    // ------------------------------------------------------------

    // CanGet

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanGet<T>() => FindFactoryEntry(typeof(T)).CanGet;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanGet<T>(object? key) => FindFactoryEntry(typeof(T), key).CanGet;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanGet(Type type) => FindFactoryEntry(type).CanGet;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool CanGet(Type type, object? key) => FindFactoryEntry(type, key).CanGet;

    // TryGet

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<T>([MaybeNullWhen(false)] out T obj)
    {
        var entry = FindFactoryEntry(typeof(T));
        if (entry.CanGet)
        {
            obj = (T)entry.Single(this);
            return true;
        }

        obj = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet<T>(object? key, [MaybeNullWhen(false)] out T obj)
    {
        var entry = FindFactoryEntry(typeof(T), key);
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
        var entry = FindFactoryEntry(type);
        if (entry.CanGet)
        {
            obj = entry.Single(this);
            return true;
        }

        obj = default;
        return false;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGet(Type type, object? key, [MaybeNullWhen(false)] out object obj)
    {
        var entry = FindFactoryEntry(type, key);
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
    public T Get<T>() => (T)FindFactoryEntry(typeof(T)).Single(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public T Get<T>(object? key) => (T)FindFactoryEntry(typeof(T), key).Single(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object Get(Type type) => FindFactoryEntry(type).Single(this);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public object Get(Type type, object? key) => FindFactoryEntry(type, key).Single(this);

    // GetAll

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<T> GetAll<T>() => FindFactoryEntry(typeof(T)).Multiple.Select(x => (T)x(this));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<T> GetAll<T>(object? key) => FindFactoryEntry(typeof(T), key).Multiple.Select(x => (T)x(this));

    public IEnumerable<object> GetAll(Type type) => FindFactoryEntry(type).Multiple.Select(x => x(this));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public IEnumerable<object> GetAll(Type type, object? key) => FindFactoryEntry(type, key).Multiple.Select(x => x(this));

    // ------------------------------------------------------------
    // Binding
    // ------------------------------------------------------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FactoryEntry FindFactoryEntry(Type type)
    {
        if (!factoriesCache.TryGetValue(type, out var entry))
        {
            entry = factoriesCache.AddIfNotExist(type, t => CreateFactoryEntry(t, false, null, this));
        }

        return entry;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FactoryEntry FindFactoryEntry(Type type, object? key)
    {
        if (!factoriesCacheWithConstraint.TryGetValue(type, key, out var entry))
        {
            entry = factoriesCacheWithConstraint.AddIfNotExist(type, key, (t, p) => CreateFactoryEntry(t, true, p, this));
        }

        return entry;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FactoryEntry FindFactoryEntry(IResolver resolver, Type type)
    {
        if (!factoriesCache.TryGetValue(type, out var entry))
        {
            entry = factoriesCache.AddIfNotExist(type, t => CreateFactoryEntry(t, false, null, resolver));
        }

        return entry;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal FactoryEntry FindFactoryEntry(IResolver resolver, Type type, object? key)
    {
        if (!factoriesCacheWithConstraint.TryGetValue(type, key, out var entry))
        {
            entry = factoriesCacheWithConstraint.AddIfNotExist(type, key, (t, p) => CreateFactoryEntry(t, true, p, resolver));
        }

        return entry;
    }

    private FactoryEntry CreateFactoryEntry(Type type, bool useConstraint, object? key, IResolver resolver)
    {
        lock (sync)
        {
            var bindings = table.Get(type) ?? handlers.SelectMany(h => h.Handle(Components, table, type));
            bindings = useConstraint
                ? bindings.Where(b => b.Constraint is not null && b.Constraint.Match(b.Metadata, key))
                : bindings.Where(b => b.Constraint is null);
            var factories = bindings
                .Select(b =>
                {
                    var factory = b.Provider.CreateFactory(this, b);
                    return b.Scope is null ? factory : b.Scope.Create(() => factory(resolver));
                })
                .ToArray();

            return new FactoryEntry(
                factories.Length > 0,
                factories.Length > 0 ? factories[^1] : nullFactory,
                factories);
        }
    }

    // ------------------------------------------------------------
    // Inject
    // ------------------------------------------------------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Inject(object instance)
    {
        var actions = FindInjectors(instance.GetType());
        for (var i = 0; i < actions.Length; i++)
        {
            actions[i](this, instance);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal Action<IResolver, object>[] FindInjectors(Type type)
    {
        if (!injectorsCache.TryGetValue(type, out var actions))
        {
            actions = injectorsCache.AddIfNotExist(type, CreateInjectors);
        }

        return actions;
    }

    private Action<IResolver, object>[] CreateInjectors(Type type)
    {
        var binding = new Binding(type, null!);
        return injectors
            .Select(x => x.CreateInjector(type, binding))
            .WhereNotNull()
            .ToArray();
    }

    // ------------------------------------------------------------
    // Scope
    // ------------------------------------------------------------

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SmartChildResolver CreateChildResolver() => new(this);

    // ------------------------------------------------------------
    // Diagnostics
    // ------------------------------------------------------------

    public DiagnosticsInfo Diagnostics
    {
        get
        {
            var factoryDiagnostics = factoriesCache.Diagnostics;
            var constraintFactoryDiagnostics = factoriesCacheWithConstraint.Diagnostics;
            var injectorDiagnostics = injectorsCache.Diagnostics;

            return new DiagnosticsInfo(
                factoryDiagnostics.Count,
                factoryDiagnostics.Width,
                factoryDiagnostics.Depth,
                constraintFactoryDiagnostics.Count,
                constraintFactoryDiagnostics.Width,
                constraintFactoryDiagnostics.Depth,
                injectorDiagnostics.Count,
                injectorDiagnostics.Width,
                injectorDiagnostics.Depth);
        }
    }

    public sealed class DiagnosticsInfo
    {
        public int FactoryCacheCount { get; }

        public int FactoryCacheWidth { get; }

        public int FactoryCacheDepth { get; }

        public int ConstraintFactoryCacheCount { get; }

        public int ConstraintFactoryCacheWidth { get; }

        public int ConstraintFactoryCacheDepth { get; }

        public int InjectorCacheCount { get; }

        public int InjectorCacheWidth { get; }

        public int InjectorCacheDepth { get; }

        public DiagnosticsInfo(
            int factoryCacheCount,
            int factoryCacheWidth,
            int factoryCacheDepth,
            int constraintFactoryCacheCount,
            int constraintFactoryCacheWidth,
            int constraintFactoryCacheDepth,
            int injectorCacheCount,
            int injectorCacheWidth,
            int injectorCacheDepth)
        {
            FactoryCacheCount = factoryCacheCount;
            FactoryCacheWidth = factoryCacheWidth;
            FactoryCacheDepth = factoryCacheDepth;
            ConstraintFactoryCacheCount = constraintFactoryCacheCount;
            ConstraintFactoryCacheWidth = constraintFactoryCacheWidth;
            ConstraintFactoryCacheDepth = constraintFactoryCacheDepth;
            InjectorCacheCount = injectorCacheCount;
            InjectorCacheWidth = injectorCacheWidth;
            InjectorCacheDepth = injectorCacheDepth;
        }
    }
}
