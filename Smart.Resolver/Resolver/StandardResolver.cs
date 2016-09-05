namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Activators;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Constraints;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;
    using Smart.Resolver.Scopes;

    /// <summary>
    ///
    /// </summary>
    public class StandardResolver : DisposableObject, IBindingRoot, IKernel
    {
        private static readonly Type ResolverType = typeof(IResolver);

        private static readonly BindingMetadata EmptyBindingMetadata = new BindingMetadata();

        private readonly ComponentContainer components = new ComponentContainer();

        private readonly Dictionary<Type, IList<IBinding>> bindings = new Dictionary<Type, IList<IBinding>>();

        /// <summary>
        ///
        /// </summary>
        public IComponentContainer Components => components;

        /// <summary>
        ///
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:DisposeObjectsBeforeLosingScope", Justification = "Ignore")]
        public StandardResolver()
        {
            components.Register<IMetadataFactory>(new MetadataFactory());
            components.Register<IActivatePipeline>(new ActivatePipeline(
                new InitializeActivator()));
            components.Register<IInjectPipeline>(new InjectPipeline(
                new PropertyInjector()));
            components.Register<ISingletonScopeStorage>(new SingletonScopeStorage());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components.Dispose();
            }

            base.Dispose(disposing);
        }

        // ------------------------------------------------------------
        // Configuration
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public StandardResolver Configure(Action<ComponentContainer> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            action(components);
            return this;
        }

        // ------------------------------------------------------------
        // IBindingRoot
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IBindingToSyntax<T> Bind<T>()
        {
            var type = typeof(T);

            var metadata = new BindingMetadata();
            var binding = new Binding(type, metadata);

            AddBinding(binding);

            return new BindingBuilder<T>(binding, metadata);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IBindingToSyntax<object> Bind(Type type)
        {
            var metadata = new BindingMetadata();
            var binding = new Binding(type, metadata);

            AddBinding(binding);

            return new BindingBuilder<object>(binding, metadata);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="binding"></param>
        private void AddBinding(IBinding binding)
        {
            lock (bindings)
            {
                IList<IBinding> list;
                if (!bindings.TryGetValue(binding.Type, out list))
                {
                    list = new List<IBinding>();
                    bindings[binding.Type] = list;
                }

                list.Add(binding);
            }
        }

        // ------------------------------------------------------------
        // IResolver
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public object Resolve(Type type, IConstraint constraint)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            if (ResolverType.IsAssignableFrom(type))
            {
                return this;
            }

            var list = GetBindings(type);
            IBinding target = null;

            if (constraint == null)
            {
                if (list.Count == 1)
                {
                    target = list[0];
                }
                else if (list.Count > 1)
                {
                    throw new InvalidOperationException(
                        String.Format(CultureInfo.InvariantCulture, "More than one component matched. type = {0}", type.Name));
                }
            }
            else
            {
                foreach (var binding in list)
                {
                    if (!constraint.Match(binding.Metadata))
                    {
                        continue;
                    }

                    if (target != null)
                    {
                        throw new InvalidOperationException(
                            String.Format(CultureInfo.InvariantCulture, "More than one component matched. type = {0}", type.Name));
                    }

                    target = binding;
                }
            }

            if (target == null)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "No such component registerd. type = {0}", type.Name));
            }

            return Resolve(target);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public IEnumerable<object> ResolveAll(Type type, IConstraint constraint)
        {
            if (type == ResolverType)
            {
                return new[] { this };
            }

            return (constraint != null ? GetBindings(type).Where(_ => constraint.Match(_.Metadata)) : GetBindings(type))
                    .Select(Resolve);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IList<IBinding> GetBindings(Type type)
        {
            lock (bindings)
            {
                IList<IBinding> list;
                if (!bindings.TryGetValue(type, out list))
                {
                    var binding = new Binding(type, new BindingMetadata());
                    list = new List<IBinding>();
                    bindings[type] = list;
                    list.Add(binding);
                }

                return list;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        private object Resolve(IBinding binding)
        {
            object instance;

            lock (binding)
            {
                var storage = binding.Scope?.GetStorage(this);
                instance = storage?.TryGet(binding);
                if (instance != null)
                {
                    return instance;
                }

                instance = binding.Provider.Create(this, binding);

                storage?.Remember(binding, instance);
            }

            var pipeline = components.Get<IActivatePipeline>();
            pipeline?.Activate(instance);

            return instance;
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

            var metadataFactory = components.Get<IMetadataFactory>();
            var metadata = metadataFactory.GetMetadata(instance.GetType());

            var pipeline = components.Get<IInjectPipeline>();
            pipeline?.Inject(this, new Binding(instance.GetType(), EmptyBindingMetadata), metadata, instance);
        }
    }
}
