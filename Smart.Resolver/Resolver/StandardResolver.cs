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
    public class StandardResolver : DisposableObject, IKernel
    {
        private readonly ThreadsafeHashArrayMap<Type, IObjectFactory[]> factoriesCache = new ThreadsafeHashArrayMap<Type, IObjectFactory[]>();

        private readonly ThreadsafeHashArrayMap<RequestKey, IObjectFactory[]> factoriesCacheWithConstraint = new ThreadsafeHashArrayMap<RequestKey, IObjectFactory[]>();

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

            //bindingsFactory = CreateBindings;
            //instanceFactory = CreateInstance;

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
            var factories = FindFactories(type, constraint);
            return factories.Length > 0;
        }

        IObjectFactory IResolver.TryResolve(Type type, IConstraint constraint, out bool result)
        {
            var factories = FindFactories(type, constraint);
            result = factories.Length > 0;
            return result ? factories[factories.Length - 1] : null;
        }

        IObjectFactory IResolver.Resolve(Type type, IConstraint constraint)
        {
            var factories = FindFactories(type, constraint);
            if (factories.Length == 0)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "No such component registerd. type = {0}", type.Name));
            }

            return factories[factories.Length - 1];
        }

        IObjectFactory[] IResolver.ResolveAll(Type type, IConstraint constraint)
        {
            return FindFactories(type, constraint);
        }

        // ------------------------------------------------------------
        // Binding
        // ------------------------------------------------------------

        private IObjectFactory[] FindFactories(Type type, IConstraint constraint)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (constraint == null)
            {
                if (!factoriesCache.TryGetValue(type, out var factories))
                {
                    factories = factoriesCache.AddIfNotExist(type, t => CreateFactories(t, null));
                }

                return factories;
            }
            else
            {
                var key = new RequestKey(type, constraint);
                if (!factoriesCacheWithConstraint.TryGetValue(key, out var factories))
                {
                    factories = factoriesCacheWithConstraint.AddIfNotExist(key, x => CreateFactories(x.Type, x.Constraint));
                }

                return factories;
            }
        }

        private IObjectFactory[] CreateFactories(Type type, IConstraint constraint)
        {
            lock (sync)
            {
                IEnumerable<IBinding> bindings = table.Get(type);
                if (bindings == null)
                {
                    var list = new List<IBinding>();
                    for (var i = 0; i < handlers.Length; i++)
                    {
                        foreach (var binding in handlers[i].Handle(Components, table, type))
                        {
                            list.Add(binding);
                        }
                    }

                    bindings = list;
                }

                if (constraint != null)
                {
                    bindings = bindings.Where(b => constraint.Match(b.Metadata));
                }

                return bindings
                    .Select(b =>
                    {
                        var factory = b.Provider.CreateFactory(this, b);
                        return b.Scope != null ? b.Scope.Convert(this, b, factory) : factory;
                    })
                    .ToArray();
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
