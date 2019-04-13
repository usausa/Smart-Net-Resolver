namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    using Smart.Collections.Concurrent;
    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Constraints;
    using Smart.Resolver.Handlers;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Providers;

    public sealed class SmartResolver : IKernel
    {
        private sealed class FactoryEntry
        {
            public bool CanGet { get; set; }

            public Func<IResolver, object> Single { get; set; }

            public Func<IResolver, object>[] Multiple { get; set; }
        }

        private readonly ThreadsafeTypeHashArrayMap<FactoryEntry> factoriesCache = new ThreadsafeTypeHashArrayMap<FactoryEntry>();

        private readonly ThreadsafeHashArrayMap<RequestKey, FactoryEntry> factoriesCacheWithConstraint = new ThreadsafeHashArrayMap<RequestKey, FactoryEntry>(RequestKeyComparer.Default);

        private readonly ThreadsafeTypeHashArrayMap<Action<IResolver, object>[]> injectorsCache = new ThreadsafeTypeHashArrayMap<Action<IResolver, object>[]>();

        private readonly object sync = new object();

        private readonly BindingTable table = new BindingTable();

        private readonly IInjector[] injectors;

        private readonly IMissingHandler[] handlers;

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

        public bool CanGet<T>()
        {
            return FindFactoryEntry(typeof(T)).CanGet;
        }

        public bool CanGet<T>(IConstraint constraint)
        {
            return FindFactoryEntry(typeof(T), constraint).CanGet;
        }

        public bool CanGet(Type type)
        {
            return FindFactoryEntry(type).CanGet;
        }

        public bool CanGet(Type type, IConstraint constraint)
        {
            return FindFactoryEntry(type, constraint).CanGet;
        }

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

        public T Get<T>()
        {
            return (T)FindFactoryEntry(typeof(T)).Single(this);
        }

        public T Get<T>(IConstraint constraint)
        {
            return (T)FindFactoryEntry(typeof(T), constraint).Single(this);
        }

        public object Get(Type type)
        {
            return FindFactoryEntry(type).Single(this);
        }

        public object Get(Type type, IConstraint constraint)
        {
            return FindFactoryEntry(type, constraint).Single(this);
        }

        // GetAll

        public IEnumerable<T> GetAll<T>()
        {
            return FindFactoryEntry(typeof(T)).Multiple.Select(x => (T)x(this));
        }

        public IEnumerable<T> GetAll<T>(IConstraint constraint)
        {
            return FindFactoryEntry(typeof(T), constraint).Multiple.Select(x => (T)x(this));
        }

        public IEnumerable<object> GetAll(Type type)
        {
            return FindFactoryEntry(type).Multiple.Select(x => x(this));
        }

        public IEnumerable<object> GetAll(Type type, IConstraint constraint)
        {
            return FindFactoryEntry(type, constraint).Multiple.Select(x => x(this));
        }

        // ------------------------------------------------------------
        // Binding
        // ------------------------------------------------------------

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private FactoryEntry FindFactoryEntry(Type type)
        {
            if (!factoriesCache.TryGetValue(type, out var entry))
            {
                entry = factoriesCache.AddIfNotExist(type, t => CreateFactoryEntry(t, null));
            }

            return entry;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private FactoryEntry FindFactoryEntry(Type type, IConstraint constraint)
        {
            var key = new RequestKey(type, constraint);
            if (!factoriesCacheWithConstraint.TryGetValue(key, out var entry))
            {
                entry = factoriesCacheWithConstraint.AddIfNotExist(key, x => CreateFactoryEntry(x.Type, x.Constraint));
            }

            return entry;
        }

        private FactoryEntry CreateFactoryEntry(Type type, IConstraint constraint)
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
                        return b.Scope is null ? factory : b.Scope.Create(this, b, factory);
                    })
                    .ToArray();

                return new FactoryEntry
                {
                    CanGet = factories.Length > 0,
                    Single = factories.Length > 0 ? factories[factories.Length - 1] : k => null,
                    Multiple = factories
                };
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

            var type = instance.GetType();
            if (!injectorsCache.TryGetValue(type, out var actions))
            {
                actions = injectorsCache.AddIfNotExist(type, CreateTypeInjectors);
            }

            for (var i = 0; i < actions.Length; i++)
            {
                actions[i](this, instance);
            }
        }

        private Action<IResolver, object>[] CreateTypeInjectors(Type type)
        {
            var binding = new Binding(type);
            return injectors
                .Select(x => x.CreateInjector(type, this, binding))
                .Where(x => x != null)
                .ToArray();
        }
    }
}
