﻿namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Activators;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Constraints;
    using Smart.Resolver.Handlers;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    /// <summary>
    ///
    /// </summary>
    public class StandardResolver : DisposableObject, IResolver
    {
        private readonly IComponentContainer components;

        private readonly BindingTable table = new BindingTable();

        private readonly Func<Type, IBinding[]> bindingsFactory;

        private readonly Func<IBinding, object> instanceFactory;

        private readonly IMetadataFactory metadataFactory;

        private readonly IActivator[] activators;

        private readonly IInjector[] injectors;

        private readonly IMissingHandler[] handlers;

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

            bindingsFactory = CreateBindings;
            instanceFactory = CreateInstance;

            components = config.CreateComponentContainer();

            metadataFactory = components.Get<IMetadataFactory>();
            activators = components.GetAll<IActivator>().ToArray();
            injectors = components.GetAll<IInjector>().ToArray();
            handlers = components.GetAll<IMissingHandler>().ToArray();

            foreach (var group in config.CreateBindings().GroupBy(b => b.Type))
            {
                table.Add(group.Key, group.ToArray());
            }

            var selfType = typeof(IResolver);
            table.Add(selfType, new IBinding[] { new Binding(selfType, new ConstantProvider<IResolver>(this), null, null, null, null) });
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
        // IResolver
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public bool CanResolve(Type type, IConstraint constraint)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return FindBinding(type, constraint) != null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public object TryResolve(Type type, IConstraint constraint, out bool result)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var binding = FindBinding(type, constraint);
            result = binding != null;
            return result ? Resolve(binding) : null;
        }

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

            var binding = FindBinding(type, constraint);
            if (binding == null)
            {
                throw new InvalidOperationException(
                    String.Format(CultureInfo.InvariantCulture, "No such component registerd. type = {0}", type.Name));
            }

            return Resolve(binding);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        public IEnumerable<object> ResolveAll(Type type, IConstraint constraint)
        {
            if (constraint == null)
            {
                return table.GetOrAdd(type, bindingsFactory)
                    .Select(Resolve);
            }

            return table.GetOrAdd(type, bindingsFactory)
                .Where(b => constraint.Match(b.Metadata))
                .Select(Resolve);
        }

        // ------------------------------------------------------------
        // Binding
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="constraint"></param>
        /// <returns></returns>
        private IBinding FindBinding(Type type, IConstraint constraint)
        {
            var list = table.GetOrAdd(type, bindingsFactory);
            if (list.Length == 0)
            {
                return null;
            }

            if (constraint == null)
            {
                return list[list.Length - 1];
            }

            for (var i = list.Length - 1; i >= 0; i--)
            {
                if (constraint.Match(list[i].Metadata))
                {
                    return list[i];
                }
            }

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IBinding[] CreateBindings(Type type)
        {
            var list = new List<IBinding>();
            for (var i = 0; i < handlers.Length; i++)
            {
                foreach (var binding in handlers[i].Handle(table, type))
                {
                    list.Add(binding);
                }
            }

            return list.ToArray();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        private object Resolve(IBinding binding)
        {
            if (binding.Scope != null)
            {
                var storage = binding.Scope.GetStorage(this, components);
                return storage.GetOrAdd(binding, instanceFactory);
            }

            return CreateInstance(binding);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        private object CreateInstance(IBinding binding)
        {
            var instance = binding.Provider.Create(this, components, binding);

            for (var i = 0; i < activators.Length; i++)
            {
                activators[i].Activate(instance);
            }

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

            var metadata = metadataFactory.GetMetadata(instance.GetType());
            var binding = new Binding(instance.GetType());

            for (var i = 0; i < injectors.Length; i++)
            {
                injectors[i].Inject(this, binding, metadata, instance);
            }
        }
    }
}
