namespace Smart.Resolver.Configs
{
    using System;
    using System.Collections.Generic;

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

        private Func<IProvider> providerFactory;

        private Func<IScope> scopeFactory;

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

        public IBindingInNamedWithSyntax ToProvider(IProvider provider)
        {
            providerFactory = () => provider;
            return this;
        }

        public IBindingInNamedWithSyntax ToSelf()
        {
            return ToProvider(new StandardProvider(typeof(T)));
        }

        public IBindingInNamedWithSyntax To<TImplementation>()
            where TImplementation : T
        {
            return ToProvider(new StandardProvider(typeof(TImplementation)));
        }

        public IBindingInNamedWithSyntax To(Type implementationType)
        {
            return ToProvider(new StandardProvider(implementationType));
        }

        public IBindingInNamedWithSyntax ToMethod(Func<IResolver, T> factory)
        {
            return ToProvider(new CallbackProvider<T>(factory));
        }

        public IBindingInNamedWithSyntax ToConstant(T value)
        {
            return ToProvider(new ConstantProvider<T>(value));
        }

        // ------------------------------------------------------------
        // In
        // ------------------------------------------------------------

        public IBindingNamedWithSyntax InScope(IScope scope)
        {
            scopeFactory = () => scope;
            return this;
        }

        public IBindingNamedWithSyntax InTransientScope()
        {
            InScope(null);
            return this;
        }

        public IBindingNamedWithSyntax InSingletonScope()
        {
            InScope(new SingletonScope());
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

        public IBindingWithSyntax WithConstructorArgument(string name, Func<IResolver, object> factory)
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

        public IBindingWithSyntax WithPropertyValue(string name, Func<IResolver, object> factory)
        {
            WithPropertyValue(name, new CallbackParameter(factory));
            return this;
        }

        // ------------------------------------------------------------
        // Factory
        // ------------------------------------------------------------

        IBinding IBindingFactory.CreateBinding()
        {
            return CreateBinding();
        }

        protected virtual IBinding CreateBinding()
        {
            return new Binding(
                targetType,
                providerFactory?.Invoke(),
                scopeFactory?.Invoke(),
                new BindingMetadata(metadataName, metadataValues),
                new ParameterMap(constructorArguments),
                new ParameterMap(propertyValues));
        }
    }
}
