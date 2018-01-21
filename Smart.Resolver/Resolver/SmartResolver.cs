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
    using Smart.Resolver.Factories;
    using Smart.Resolver.Handlers;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;
    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    public sealed class SmartResolver : DisposableObject, IKernel
    {
        private sealed class FactoryEntry
        {
            public IObjectFactory Single { get; set; }

            public IObjectFactory[] Multiple { get; set; }
        }

        private readonly ThreadsafeTypeHashArrayMap<FactoryEntry> factoriesCache = new ThreadsafeTypeHashArrayMap<FactoryEntry>();

        private readonly ThreadsafeHashArrayMap<RequestKey, FactoryEntry> factoriesCacheWithConstraint = new ThreadsafeHashArrayMap<RequestKey, FactoryEntry>();

        private readonly object sync = new object();

        private readonly BindingTable table = new BindingTable();

        private readonly IMetadataFactory metadataFactory;

        private readonly IInjector[] injectors;

        private readonly IMissingHandler[] handlers;

        /// <summary>
        ///
        /// </summary>
        public IComponentContainer Components { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="config"></param>
        public SmartResolver(IResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            Components = config.CreateComponentContainer();

            metadataFactory = Components.Get<IMetadataFactory>();
            injectors = Components.GetAll<IInjector>().ToArray();
            handlers = Components.GetAll<IMissingHandler>().ToArray();

            foreach (var group in config.CreateBindings(Components).GroupBy(b => b.Type))
            {
                table.Add(group.Key, group.ToArray());
            }

            var selfType = typeof(SmartResolver);
            table.Add(selfType, new IBinding[] { new Binding(selfType, new ConstantProvider(this), null, null, null, null) });
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Components.Dispose();
            }

            base.Dispose(disposing);
        }

        // ------------------------------------------------------------
        // ObjectFactory
        // ------------------------------------------------------------

        IObjectFactory IKernel.ResolveFactory(Type type, IConstraint constraint)
        {
            return (constraint == null
                ? FindFactoryEntry(type)
                : FindFactoryEntry(type, constraint)).Single;
        }

        IEnumerable<IObjectFactory> IKernel.ResolveAllFactory(Type type, IConstraint constraint)
        {
            return (constraint == null
                ? FindFactoryEntry(type)
                : FindFactoryEntry(type, constraint)).Multiple;
        }

        // ------------------------------------------------------------
        // Resolver
        // ------------------------------------------------------------

        // CanGet

        public bool CanGet<T>()
        {
            return FindFactoryEntry(typeof(T)).Single != null;
        }

        public bool CanGet<T>(IConstraint constraint)
        {
            return FindFactoryEntry(typeof(T), constraint).Single != null;
        }

        public bool CanGet(Type type)
        {
            return FindFactoryEntry(type).Single != null;
        }

        public bool CanGet(Type type, IConstraint constraint)
        {
            return FindFactoryEntry(type, constraint).Single != null;
        }

        // Get

        public T Get<T>()
        {
            return (T)FindFactoryEntry(typeof(T)).Single?.Create();
        }

        public T Get<T>(IConstraint constraint)
        {
            return (T)FindFactoryEntry(typeof(T), constraint).Single?.Create();
        }

        public object Get(Type type)
        {
            return FindFactoryEntry(type).Single?.Create();
        }

        public object Get(Type type, IConstraint constraint)
        {
            return FindFactoryEntry(type, constraint).Single?.Create();
        }

        // GetAll

        public IEnumerable<T> GetAll<T>()
        {
            return FindFactoryEntry(typeof(T)).Multiple.Select(x => (T)x.Create());
        }

        public IEnumerable<T> GetAll<T>(IConstraint constraint)
        {
            return FindFactoryEntry(typeof(T), constraint).Multiple.Select(x => (T)x.Create());
        }

        public IEnumerable<object> GetAll(Type type)
        {
            return FindFactoryEntry(type).Multiple.Select(x => x.Create());
        }

        public IEnumerable<object> GetAll(Type type, IConstraint constraint)
        {
            return FindFactoryEntry(type, constraint).Multiple.Select(x => x.Create());
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
                        return b.Scope != null ? b.Scope.Create(this, b, factory) : factory;
                    })
                    .ToArray();

                return new FactoryEntry
                {
                    Single = factories.Length > 0 ? factories[factories.Length - 1] : null,
                    Multiple = factories
                };
            }
        }

        // ------------------------------------------------------------
        // Inject
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        public void Inject(object instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var type = instance.GetType();
            var metadata = metadataFactory.GetMetadata(type);
            var binding = new Binding(type);

            for (var i = 0; i < injectors.Length; i++)
            {
                if (injectors[i].IsTarget(this, binding, metadata, type))
                {
                    injectors[i].Inject(this, binding, metadata, instance);
                }
            }
        }
    }
}
