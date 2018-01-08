namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

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
    public sealed class StandardResolver : DisposableObject, IKernel
    {
        private sealed class FactoryEntry
        {
            public IObjectFactory Single { get; set; }

            public IObjectFactory[] Multi { get; set; }
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
        public StandardResolver(IResolverConfig config)
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

            var selfType = typeof(IResolver);
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
        // IResolver
        // ------------------------------------------------------------

        bool IResolver.CanResolve(Type type, IConstraint constraint)
        {
            return FindFactoryEntry(type, constraint).Single != null;
        }

        IObjectFactory IResolver.TryResolve(Type type, IConstraint constraint, out bool result)
        {
            var factory = FindFactoryEntry(type, constraint).Single;
            result = factory != null;
            return factory;
        }

        IObjectFactory IResolver.Resolve(Type type, IConstraint constraint)
        {
            var factory = FindFactoryEntry(type, constraint).Single;
            if (factory == null)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "No such component registerd. type = {0}", type.Name));
            }

            return factory;
        }

        IEnumerable<IObjectFactory> IResolver.ResolveAll(Type type, IConstraint constraint)
        {
            return FindFactoryEntry(type, constraint).Multi;
        }

        // ------------------------------------------------------------
        // Binding
        // ------------------------------------------------------------

        private FactoryEntry FindFactoryEntry(Type type, IConstraint constraint)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (constraint == null)
            {
                if (!factoriesCache.TryGetValue(type, out var entry))
                {
                    entry = factoriesCache.AddIfNotExist(type, t => CreateFactoryEntry(t, null));
                }

                return entry;
            }
            else
            {
                var key = new RequestKey(type, constraint);
                if (!factoriesCacheWithConstraint.TryGetValue(key, out var entry))
                {
                    entry = factoriesCacheWithConstraint.AddIfNotExist(key, x => CreateFactoryEntry(x.Type, x.Constraint));
                }

                return entry;
            }
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
                    Multi = factories
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

            var metadata = metadataFactory.GetMetadata(instance.GetType());
            var binding = new Binding(instance.GetType());

            for (var i = 0; i < injectors.Length; i++)
            {
                injectors[i].Inject(this, binding, metadata, instance);
            }
        }
    }
}
