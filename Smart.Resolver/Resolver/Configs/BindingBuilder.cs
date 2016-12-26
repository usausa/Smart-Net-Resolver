namespace Smart.Resolver.Configs
{
    using System;
    using System.Collections.Generic;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BindingBuilder<T> : IBindingFactory, IBindingToInNamedWithSyntax<T>
    {
        private readonly Type targetType;

        private Func<IComponentContainer, IProvider> providerFactory;

        private Func<IComponentContainer, IScope> scopeFactory;

        private string metadataName;

        private Dictionary<string, object> metadataValues;

        private Dictionary<string, IParameter> constructorArguments;

        private Dictionary<string, IParameter> propertyValues;

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
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
            return ToProvider(c => new StandardProvider(typeof(T), c));
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

        public IBindingInNamedWithSyntax ToMethod(Func<IKernel, T> factory)
        {
            return ToProvider(c => new CallbackProvider<T>(factory));
        }

        public IBindingInNamedWithSyntax ToConstant(T value)
        {
            return ToProvider(c => new ConstantProvider<T>(value));
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
            if (metadataValues == null)
            {
                metadataValues = new Dictionary<string, object>();
            }

            metadataValues[key] = value;
            return this;
        }

        public IBindingWithSyntax WithConstructorArgument(string name, IParameter parameter)
        {
            if (constructorArguments == null)
            {
                constructorArguments = new Dictionary<string, IParameter>();
            }

            constructorArguments[name] = parameter;
            return this;
        }

        public IBindingWithSyntax WithConstructorArgument(string name, object value)
        {
            WithConstructorArgument(name, new ConstantParameter(value));
            return this;
        }

        public IBindingWithSyntax WithConstructorArgument(string name, Func<IKernel, object> factory)
        {
            WithConstructorArgument(name, new CallbackParameter(factory));
            return this;
        }

        public IBindingWithSyntax WithPropertyValue(string name, IParameter parameter)
        {
            if (propertyValues == null)
            {
                propertyValues = new Dictionary<string, IParameter>();
            }

            propertyValues[name] = parameter;
            return this;
        }

        public IBindingWithSyntax WithPropertyValue(string name, object value)
        {
            WithPropertyValue(name, new ConstantParameter(value));
            return this;
        }

        public IBindingWithSyntax WithPropertyValue(string name, Func<IKernel, object> factory)
        {
            WithPropertyValue(name, new CallbackParameter(factory));
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
                new ParameterMap(constructorArguments),
                new ParameterMap(propertyValues));
        }
    }
}
