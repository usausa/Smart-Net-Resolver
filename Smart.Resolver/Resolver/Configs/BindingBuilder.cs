namespace Smart.Resolver.Configs
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    public class BindingBuilder<T> : IBindingFactory, IBindingToInNamedWithSyntax<T>
    {
        private readonly Type targetType;

        private Func<IComponentContainer, IProvider> providerFactory;

        private Func<IComponentContainer, IScope> scopeFactory;

        private string metadataName;

        private Dictionary<string, object> metadataValues;

        private Dictionary<string, Func<IComponentContainer, IParameter>> constructorArgumentFactories;

        private Dictionary<string, Func<IComponentContainer, IParameter>> propertyValueFactories;

        public BindingBuilder(Type type)
        {
            targetType = type;
        }

        // ------------------------------------------------------------
        // To
        // ------------------------------------------------------------

        public IBindingInNamedWithSyntax ToProvider(Func<IComponentContainer, IProvider> factory)
        {
            providerFactory = factory;
            return this;
        }

        public IBindingInNamedWithSyntax ToSelf()
        {
            return ToProvider(c => new StandardProvider(targetType, c));
        }

        public IBindingInNamedWithSyntax To<TImplementation>()
            where TImplementation : T
        {
            return ToProvider(c => new StandardProvider(typeof(TImplementation), c));
        }

        public IBindingInNamedWithSyntax To(Type implementationType)
        {
            return ToProvider(c => new StandardProvider(implementationType, c));
        }

        public IBindingInNamedWithSyntax ToMethod(Func<IResolver, T> factory)
        {
            return ToProvider(c => new CallbackProvider(targetType, resolver => factory(resolver)));
        }

        public IBindingInNamedWithSyntax ToConstant(T value)
        {
            return ToProvider(c => new ConstantProvider(value));
        }

        // ------------------------------------------------------------
        // In
        // ------------------------------------------------------------

        public IBindingNamedWithSyntax InScope(Func<IComponentContainer, IScope> factory)
        {
            scopeFactory = factory;
            return this;
        }

        public IBindingNamedWithSyntax InTransientScope()
        {
            InScope(null);
            return this;
        }

        public IBindingNamedWithSyntax InSingletonScope()
        {
            InScope(c => new SingletonScope(c));
            return this;
        }

        public IBindingNamedWithSyntax InContainerScope()
        {
            InScope(c => new ContainerScope());
            return this;
        }

        // ------------------------------------------------------------
        // Named
        // ------------------------------------------------------------

        public IBindingWithSyntax Named(string name)
        {
            metadataName = name;
            return this;
        }

        // ------------------------------------------------------------
        // With
        // ------------------------------------------------------------

        public IBindingWithSyntax WithMetadata(string key, object value)
        {
            if (metadataValues is null)
            {
                metadataValues = new Dictionary<string, object>();
            }

            metadataValues[key] = value;
            return this;
        }

        public IBindingWithSyntax WithConstructorArgument(string name, Func<IComponentContainer, IParameter> factory)
        {
            if (constructorArgumentFactories is null)
            {
                constructorArgumentFactories = new Dictionary<string, Func<IComponentContainer, IParameter>>();
            }

            constructorArgumentFactories[name] = factory;
            return this;
        }

        public IBindingWithSyntax WithConstructorArgument(string name, object value)
        {
            WithConstructorArgument(name, c => new ConstantParameter(value));
            return this;
        }

        public IBindingWithSyntax WithConstructorArgument(string name, Func<IResolver, object> factory)
        {
            WithConstructorArgument(name, c => new CallbackParameter(factory));
            return this;
        }

        public IBindingWithSyntax WithPropertyValue(string name, Func<IComponentContainer, IParameter> factory)
        {
            if (propertyValueFactories is null)
            {
                propertyValueFactories = new Dictionary<string, Func<IComponentContainer, IParameter>>();
            }

            propertyValueFactories[name] = factory;
            return this;
        }

        public IBindingWithSyntax WithPropertyValue(string name, object value)
        {
            WithPropertyValue(name, c => new ConstantParameter(value));
            return this;
        }

        public IBindingWithSyntax WithPropertyValue(string name, Func<IResolver, object> factory)
        {
            WithPropertyValue(name, c => new CallbackParameter(factory));
            return this;
        }

        // ------------------------------------------------------------
        // Factory
        // ------------------------------------------------------------

        IBinding IBindingFactory.CreateBinding(IComponentContainer components)
        {
            return CreateBinding(components);
        }

        protected virtual IBinding CreateBinding(IComponentContainer components)
        {
            return new Binding(
                targetType,
                providerFactory?.Invoke(components),
                scopeFactory?.Invoke(components),
                new BindingMetadata(metadataName, metadataValues),
                new ParameterMap(constructorArgumentFactories?.ToDictionary(kv => kv.Key, kv => kv.Value(components))),
                new ParameterMap(propertyValueFactories?.ToDictionary(kv => kv.Key, kv => kv.Value(components))));
        }
    }
}
