namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Reflection;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Configs;
    using Smart.Resolver.Disposables;
    using Smart.Resolver.Metadatas;

    /// <summary>
    ///
    /// </summary>
    public class ResolverConfig : IResolverConfig, IBindingRoot
    {
        public ComponentConfig Components { get; } = new ComponentConfig();

        private readonly List<IBindingFactory> bindingFactories = new List<IBindingFactory>();

        /// <summary>
        ///
        /// </summary>
        public ResolverConfig()
        {
            Components.Add<IDelegateFactory>(DelegateFactory.Default);
            Components.Add<IMetadataFactory, MetadataFactory>();
            Components.Add<DisposableStorage>();
        }

        // ------------------------------------------------------------
        // IResolverConfig
        // ------------------------------------------------------------

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IComponentContainer IResolverConfig.CreateComponentContainer()
        {
            return CreateComponentContainer();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IComponentContainer CreateComponentContainer()
        {
            return Components.ToContainer();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        IEnumerable<IBinding> IResolverConfig.CreateBindings(IComponentContainer components)
        {
            return CreateBindings(components);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        protected virtual IEnumerable<IBinding> CreateBindings(IComponentContainer components)
        {
            return bindingFactories.Select(f => f.CreateBinding(components));
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
            var builder = new BindingBuilder<T>(typeof(T));
            bindingFactories.Add(builder);
            return builder;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IBindingToSyntax<object> Bind(Type type)
        {
            var builder = new BindingBuilder<object>(type);
            bindingFactories.Add(builder);
            return builder;
        }
    }
}
