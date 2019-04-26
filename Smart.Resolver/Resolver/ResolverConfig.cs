namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Reflection;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Builders;
    using Smart.Resolver.Components;
    using Smart.Resolver.Configs;

    public class ResolverConfig : IResolverConfig, IBindingRoot
    {
        public ComponentConfig Components { get; } = new ComponentConfig();

        private readonly List<IBindingFactory> bindingFactories = new List<IBindingFactory>();

        public ResolverConfig()
        {
            Components.Add<DisposableStorage>();
            Components.Add<IDelegateFactory>(DelegateFactory.Default);
            if (DelegateFactory.Default.IsCodegenRequired)
            {
                Components.Add<IFactoryBuilder, EmitFactoryBuilder>();
            }
            else
            {
                Components.Add<IFactoryBuilder, ReflectionFactoryBuilder>();
            }
        }

        // ------------------------------------------------------------
        // IResolverConfig
        // ------------------------------------------------------------

        IComponentContainer IResolverConfig.CreateComponentContainer()
        {
            return CreateComponentContainer();
        }

        protected virtual IComponentContainer CreateComponentContainer()
        {
            return Components.ToContainer();
        }

        IEnumerable<IBinding> IResolverConfig.CreateBindings(IComponentContainer components)
        {
            return CreateBindings(components);
        }

        protected virtual IEnumerable<IBinding> CreateBindings(IComponentContainer components)
        {
            return bindingFactories.Select(f => f.CreateBinding(components));
        }

        // ------------------------------------------------------------
        // IBindingRoot
        // ------------------------------------------------------------

        public IBindingToSyntax<T> Bind<T>()
        {
            var builder = new BindingBuilder<T>(typeof(T));
            bindingFactories.Add(builder);
            return builder;
        }

        public IBindingToSyntax<object> Bind(Type type)
        {
            var builder = new BindingBuilder<object>(type);
            bindingFactories.Add(builder);
            return builder;
        }
    }
}
