namespace Smart.Resolver.Bindings
{
    using System;

    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BindingBuilder<T> : IBindingToInNamedWithSyntax<T>
    {
        protected Binding Binding { get; }

        protected BindingMetadata Metadata { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="metadata"></param>
        public BindingBuilder(Binding binding, BindingMetadata metadata)
        {
            Binding = binding;
            Metadata = metadata;
        }

        // ------------------------------------------------------------
        // To
        // ------------------------------------------------------------

        public IBindingInNamedWithSyntax ToProvider(IProvider provider)
        {
            Binding.Provider = provider;
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

        public IBindingInNamedWithSyntax ToMethod(Func<IKernel, T> factory)
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
            Binding.Scope = scope;
            return this;
        }

        public IBindingNamedWithSyntax InTransientScope()
        {
            Binding.Scope = null;
            return this;
        }

        public IBindingNamedWithSyntax InSingletonScope()
        {
            Binding.Scope = new SingletonScope();
            return this;
        }

        // ------------------------------------------------------------
        // Named
        // ------------------------------------------------------------

        public IBindingWithSyntax Named(string name)
        {
            Metadata.Name = name;
            return this;
        }

        // ------------------------------------------------------------
        // With
        // ------------------------------------------------------------

        public IBindingWithSyntax WithConstructorArgument(string name, object value)
        {
            Binding.AddConstructorArgument(name, new ConstantParameter(value));
            return this;
        }

        public IBindingWithSyntax WithConstructorArgument(string name, Func<IKernel, object> factory)
        {
            Binding.AddConstructorArgument(name, new CallbackParameter(factory));
            return this;
        }

        public IBindingWithSyntax WithPropertyValue(string name, object value)
        {
            Binding.AddPropertyValue(name, new ConstantParameter(value));
            return this;
        }

        public IBindingWithSyntax WithPropertyValue(string name, Func<IKernel, object> factory)
        {
            Binding.AddPropertyValue(name, new CallbackParameter(factory));
            return this;
        }

        public IBindingWithSyntax WithMetadata(string key, object value)
        {
            Metadata.Set(key, value);
            return this;
        }
    }
}
