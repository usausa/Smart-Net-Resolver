namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;
    using System.Threading;

    using Smart.Collections.Concurrent;
    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Constraints;
    using Smart.Resolver.Handlers;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Providers;

    public sealed class SmartResolver : IResolver, IKernel
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401:FieldsMustBePrivate", Justification = "Performance")]
        private sealed class FactoryEntry
        {
            public readonly bool CanGet;

            public readonly Func<IResolver, object> Single;

            public readonly Func<IResolver, object>[] Multiple;

            public FactoryEntry(bool canGet, Func<IResolver, object> single, Func<IResolver, object>[] multiple)
            {
                CanGet = canGet;
                Single = single;
                Multiple = multiple;
            }
        }

        private readonly ThreadsafeTypeHashArrayMap<FactoryEntry> factoriesCache = new ThreadsafeTypeHashArrayMap<FactoryEntry>(128);

        private readonly TypeConstraintHashArray<FactoryEntry> factoriesCacheWithConstraint = new TypeConstraintHashArray<FactoryEntry>();

        private readonly ThreadsafeTypeHashArrayMap<Action<IResolver, object>[]> injectorsCache = new ThreadsafeTypeHashArrayMap<Action<IResolver, object>[]>();

        private readonly object sync = new object();

        private readonly BindingTable table = new BindingTable();

        private readonly IInjector[] injectors;

        private readonly IMissingHandler[] handlers;

        private int disposed;

        public IComponentContainer Components { get; }

        public SmartResolver(IResolverConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            Components = config.CreateComponentContainer();

            injectors = Components.GetAll<IInjector>().ToArray();
            handlers = Components.GetAll<IMissingHandler>().ToArray();

            foreach (var group in config.CreateBindings(Components).GroupBy(b => b.Type))
            {
                table.Add(group.Key, group.ToArray());
            }

            table.Add(typeof(IResolver), new IBinding[] { new Binding(typeof(IResolver), new ConstantProvider(this), null, null, null, null) });
            table.Add(typeof(SmartResolver), new IBinding[] { new Binding(typeof(SmartResolver), new ConstantProvider(this), null, null, null, null) });
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
        // ObjectFactory
        // ------------------------------------------------------------

        bool IKernel.TryResolveFactory(Type type, IConstraint constraint, out Func<IResolver, object> factory)
        {
            var entry = constraint is null ? FindFactoryEntry(type) : FindFactoryEntry(type, constraint);
            factory = entry.Single;
            return entry.CanGet;
        }

        bool IKernel.TryResolveFactories(Type type, IConstraint constraint, out Func<IResolver, object>[] factories)
        {
            var entry = constraint is null ? FindFactoryEntry(type) : FindFactoryEntry(type, constraint);
            factories = entry.Multiple;
            return entry.CanGet;
        }

        // ------------------------------------------------------------
        // Resolver
        // ------------------------------------------------------------

        // CanGet

        public bool CanGet<T>() =>
            FindFactoryEntry(typeof(T)).CanGet;

        public bool CanGet<T>(IConstraint constraint) =>
            FindFactoryEntry(typeof(T), constraint).CanGet;

        public bool CanGet(Type type) =>
            FindFactoryEntry(type).CanGet;

        public bool CanGet(Type type, IConstraint constraint) =>
            FindFactoryEntry(type, constraint).CanGet;

        // TryGet

        public bool TryGet<T>(out T obj)
        {
            var entry = FindFactoryEntry(typeof(T));
            obj = entry.CanGet ? (T)entry.Single(this) : default;
            return entry.CanGet;
        }

        public bool TryGet<T>(IConstraint constraint, out T obj)
        {
            var entry = FindFactoryEntry(typeof(T), constraint);
            obj = entry.CanGet ? (T)entry.Single(this) : default;
            return entry.CanGet;
        }

        public bool TryGet(Type type, out object obj)
        {
            var entry = FindFactoryEntry(type);
            obj = entry.CanGet ? entry.Single(this) : default;
            return entry.CanGet;
        }

        public bool TryGet(Type type, IConstraint constraint, out object obj)
        {
            var entry = FindFactoryEntry(type, constraint);
            obj = entry.CanGet ? entry.Single(this) : default;
            return entry.CanGet;
        }

        // Get

        public T Get<T>() =>
            (T)FindFactoryEntry(typeof(T)).Single(this);

        public T Get<T>(IConstraint constraint) =>
            (T)FindFactoryEntry(typeof(T), constraint).Single(this);

        public object Get(Type type) =>
            FindFactoryEntry(type).Single(this);

        public object Get(Type type, IConstraint constraint) =>
            FindFactoryEntry(type, constraint).Single(this);

        // GetAll

        public IEnumerable<T> GetAll<T>() =>
            FindFactoryEntry(typeof(T)).Multiple.Select(x => (T)x(this));

        public IEnumerable<T> GetAll<T>(IConstraint constraint) =>
            FindFactoryEntry(typeof(T), constraint).Multiple.Select(x => (T)x(this));

        public IEnumerable<object> GetAll(Type type) =>
            FindFactoryEntry(type).Multiple.Select(x => x(this));

        public IEnumerable<object> GetAll(Type type, IConstraint constraint) =>
            FindFactoryEntry(type, constraint).Multiple.Select(x => x(this));

        // ------------------------------------------------------------
        // Binding
        // ------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private FactoryEntry FindFactoryEntry(Type type)
        {
            if (!factoriesCache.TryGetValue(type, out var entry))
            {
                entry = factoriesCache.AddIfNotExist(type, t => CreateFactoryEntry(t, null, this));
            }

            return entry;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private FactoryEntry FindFactoryEntry(Type type, IConstraint constraint)
        {
            if (!factoriesCacheWithConstraint.TryGetValue(type, constraint, out var entry))
            {
                entry = factoriesCacheWithConstraint.AddIfNotExist(type, constraint, (t, c) => CreateFactoryEntry(t, c, this));
            }

            return entry;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private FactoryEntry FindFactoryEntry(IResolver resolver, Type type)
        {
            if (!factoriesCache.TryGetValue(type, out var entry))
            {
                entry = factoriesCache.AddIfNotExist(type, t => CreateFactoryEntry(t, null, resolver));
            }

            return entry;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private FactoryEntry FindFactoryEntry(IResolver resolver, Type type, IConstraint constraint)
        {
            if (!factoriesCacheWithConstraint.TryGetValue(type, constraint, out var entry))
            {
                entry = factoriesCacheWithConstraint.AddIfNotExist(type, constraint, (t, c) => CreateFactoryEntry(t, c, resolver));
            }

            return entry;
        }

        private FactoryEntry CreateFactoryEntry(Type type, IConstraint constraint, IResolver resolver)
        {
            lock (sync)
            {
                var bindings = table.Get(type) ?? handlers.SelectMany(h => h.Handle(Components, table, type));
                if (constraint != null)
                {
                    bindings = bindings.Where(b => constraint.Match(b.Metadata));
                }

                var factories = bindings
                    .Select(b =>
                    {
                        var factory = b.Provider.CreateFactory(this, b);
                        return b.Scope is null ? factory : b.Scope.Create(b, () => factory(resolver));
                    })
                    .ToArray();

                return new FactoryEntry(
                    factories.Length > 0,
                    factories.Length > 0 ? factories[factories.Length - 1] : k => null,
                    factories);
            }
        }

        // ------------------------------------------------------------
        // Inject
        // ------------------------------------------------------------

        public void Inject(object instance)
        {
            if (instance is null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var actions = FindInjectors(instance.GetType());
            for (var i = 0; i < actions.Length; i++)
            {
                actions[i](this, instance);
            }
        }

        private Action<IResolver, object>[] FindInjectors(Type type)
        {
            if (!injectorsCache.TryGetValue(type, out var actions))
            {
                actions = injectorsCache.AddIfNotExist(type, CreateInjectors);
            }

            return actions;
        }

        private Action<IResolver, object>[] CreateInjectors(Type type)
        {
            var binding = new Binding(type);
            return injectors
                .Select(x => x.CreateInjector(type, binding))
                .Where(x => x != null)
                .ToArray();
        }

        // ------------------------------------------------------------
        // Scope
        // ------------------------------------------------------------

        public IResolver CreateChildResolver()
        {
            return new ChildResolver(this);
        }

        private sealed class ChildResolver : IResolver, IContainer
        {
            private readonly SmartResolver resolver;

            private readonly Dictionary<IBinding, object> cache = new Dictionary<IBinding, object>();

            public ChildResolver(SmartResolver resolver)
            {
                this.resolver = resolver;
            }

            public void Dispose()
            {
                lock (cache)
                {
                    foreach (var obj in cache.Values)
                    {
                        (obj as IDisposable)?.Dispose();
                    }

                    cache.Clear();
                }
            }

            public object Create(IBinding binding, Func<object> factory)
            {
                lock (cache)
                {
                    if (!cache.TryGetValue(binding, out var value))
                    {
                        value = factory();
                        cache[binding] = value;
                    }

                    return value;
                }
            }

            // CanGet

            public bool CanGet<T>() =>
                resolver.FindFactoryEntry(this, typeof(T)).CanGet;

            public bool CanGet<T>(IConstraint constraint) =>
                resolver.FindFactoryEntry(this, typeof(T), constraint).CanGet;

            public bool CanGet(Type type) =>
                resolver.FindFactoryEntry(this, type).CanGet;

            public bool CanGet(Type type, IConstraint constraint) =>
                resolver.FindFactoryEntry(this, type, constraint).CanGet;

            // TryGet

            public bool TryGet<T>(out T obj)
            {
                var entry = resolver.FindFactoryEntry(this, typeof(T));
                obj = entry.CanGet ? (T)entry.Single(this) : default;
                return entry.CanGet;
            }

            public bool TryGet<T>(IConstraint constraint, out T obj)
            {
                var entry = resolver.FindFactoryEntry(this, typeof(T), constraint);
                obj = entry.CanGet ? (T)entry.Single(this) : default;
                return entry.CanGet;
            }

            public bool TryGet(Type type, out object obj)
            {
                var entry = resolver.FindFactoryEntry(this, type);
                obj = entry.CanGet ? entry.Single(this) : default;
                return entry.CanGet;
            }

            public bool TryGet(Type type, IConstraint constraint, out object obj)
            {
                var entry = resolver.FindFactoryEntry(this, type, constraint);
                obj = entry.CanGet ? entry.Single(this) : default;
                return entry.CanGet;
            }

            // Get

            public T Get<T>() =>
                (T)resolver.FindFactoryEntry(this, typeof(T)).Single(this);

            public T Get<T>(IConstraint constraint) =>
                (T)resolver.FindFactoryEntry(this, typeof(T), constraint).Single(this);

            public object Get(Type type) =>
                resolver.FindFactoryEntry(this, type).Single(this);

            public object Get(Type type, IConstraint constraint) =>
                resolver.FindFactoryEntry(this, type, constraint).Single(this);

            // GetAll

            public IEnumerable<T> GetAll<T>() =>
                resolver.FindFactoryEntry(this, typeof(T)).Multiple.Select(x => (T)x(this));

            public IEnumerable<T> GetAll<T>(IConstraint constraint) =>
                resolver.FindFactoryEntry(this, typeof(T), constraint).Multiple.Select(x => (T)x(this));

            public IEnumerable<object> GetAll(Type type) =>
                resolver.FindFactoryEntry(this, type).Multiple.Select(x => x(this));

            public IEnumerable<object> GetAll(Type type, IConstraint constraint) =>
                resolver.FindFactoryEntry(this, type, constraint).Multiple.Select(x => x(this));

            public void Inject(object instance)
            {
                if (instance is null)
                {
                    throw new ArgumentNullException(nameof(instance));
                }

                var actions = resolver.FindInjectors(instance.GetType());
                for (var i = 0; i < actions.Length; i++)
                {
                    actions[i](this, instance);
                }
            }
        }

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
}
