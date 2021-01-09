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
    using Smart.Resolver.Components;
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

        private readonly ThreadsafeTypeHashArrayMap<FactoryEntry> factoriesCache = new(128);

        private readonly TypeConstraintHashArray<FactoryEntry> factoriesCacheWithConstraint = new();

        private readonly ThreadsafeTypeHashArrayMap<Action<IResolver, object>[]> injectorsCache = new();

        private readonly Func<IResolver, object> nullFactory = _ => null;

        private readonly object sync = new();

        private readonly BindingTable table;

        private readonly IInjector[] injectors;

        private readonly IMissingHandler[] handlers;

        private int disposed;

        public ComponentContainer Components { get; }

        public SmartResolver(IResolverConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            Components = config.CreateComponentContainer();

            injectors = Components.GetAll<IInjector>().ToArray();
            handlers = Components.GetAll<IMissingHandler>().ToArray();

            var tableEntries = new Dictionary<Type, Binding[]>();

            foreach (var group in config.CreateBindings(Components).GroupBy(b => b.Type))
            {
                tableEntries.Add(group.Key, group.ToArray());
            }

            tableEntries.Add(typeof(IResolver), new[] { new Binding(typeof(IResolver), new ConstantProvider(this), null, null, null, null) });
            tableEntries.Add(typeof(SmartResolver), new[] { new Binding(typeof(SmartResolver), new ConstantProvider(this), null, null, null, null) });

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

        public bool CanGet<T>() => FindFactoryEntry(typeof(T)).CanGet;

        public bool CanGet<T>(IConstraint constraint) => FindFactoryEntry(typeof(T), constraint).CanGet;

        public bool CanGet(Type type) => FindFactoryEntry(type).CanGet;

        public bool CanGet(Type type, IConstraint constraint) => FindFactoryEntry(type, constraint).CanGet;

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

        public T Get<T>() => (T)FindFactoryEntry(typeof(T)).Single(this);

        public T Get<T>(IConstraint constraint) => (T)FindFactoryEntry(typeof(T), constraint).Single(this);

        public object Get(Type type) => FindFactoryEntry(type).Single(this);

        public object Get(Type type, IConstraint constraint) => FindFactoryEntry(type, constraint).Single(this);

        // GetAll

        public IEnumerable<T> GetAll<T>() => FindFactoryEntry(typeof(T)).Multiple.Select(x => (T)x(this));

        public IEnumerable<T> GetAll<T>(IConstraint constraint) => FindFactoryEntry(typeof(T), constraint).Multiple.Select(x => (T)x(this));

        public IEnumerable<object> GetAll(Type type) => FindFactoryEntry(type).Multiple.Select(x => x(this));

        public IEnumerable<object> GetAll(Type type, IConstraint constraint) => FindFactoryEntry(type, constraint).Multiple.Select(x => x(this));

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
                if (constraint is not null)
                {
                    bindings = bindings.Where(b => constraint.Match(b.Metadata));
                }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
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
                .Where(x => x is not null)
                .ToArray();
        }

        // ------------------------------------------------------------
        // Scope
        // ------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public IResolver CreateChildResolver() => new ChildResolver(this);

        private sealed class ChildResolver : IResolver, IContainer
        {
            [ThreadStatic]
            private static ContainerSlot pool;

            private readonly SmartResolver resolver;

            public ContainerSlot Slot { get; }

            public ChildResolver(SmartResolver resolver)
            {
                this.resolver = resolver;

                if (pool is null)
                {
                    Slot = new ContainerSlot();
                }
                else
                {
                    Slot = pool;
                    pool = null;
                }
            }

            public void Dispose()
            {
                Slot.Clear();
                pool ??= Slot;
            }

            // CanGet

            public bool CanGet<T>() => resolver.FindFactoryEntry(this, typeof(T)).CanGet;

            public bool CanGet<T>(IConstraint constraint) => resolver.FindFactoryEntry(this, typeof(T), constraint).CanGet;

            public bool CanGet(Type type) => resolver.FindFactoryEntry(this, type).CanGet;

            public bool CanGet(Type type, IConstraint constraint) => resolver.FindFactoryEntry(this, type, constraint).CanGet;

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
