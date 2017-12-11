namespace Smart.Resolver.Bindings
{
    using System;

    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    /// <summary>
    ///
    /// </summary>
    public sealed class Binding : IBinding
    {
        private static readonly IBindingMetadata EmptyBindingMetadata = new BindingMetadata();

        /// <summary>
        ///
        /// </summary>
        public Type Type { get; }

        /// <summary>
        ///
        /// </summary>
        public IProvider Provider { get; }

        /// <summary>
        ///
        /// </summary>
        public IScope Scope { get; }

        /// <summary>
        ///
        /// </summary>
        public IBindingMetadata Metadata { get; }

        /// <summary>
        ///
        /// </summary>
        public ParameterMap ConstructorArguments { get; }

        /// <summary>
        ///
        /// </summary>
        public ParameterMap PropertyValues { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        public Binding(Type type)
            : this(type, null, null, null, null, null)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="provider"></param>
        public Binding(Type type, IProvider provider)
            : this(type, provider, null, null, null, null)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="metadata"></param>
        /// <param name="provider"></param>
        /// <param name="scope"></param>
        /// <param name="constructorArguments"></param>
        /// <param name="propertyValues"></param>
        public Binding(Type type, IProvider provider, IScope scope, IBindingMetadata metadata, ParameterMap constructorArguments, ParameterMap propertyValues)
        {
            Type = type;
            Provider = provider;
            Scope = scope;
            Metadata = metadata ?? EmptyBindingMetadata;
            ConstructorArguments = constructorArguments ?? new ParameterMap(null);
            PropertyValues = propertyValues ?? new ParameterMap(null);
        }
    }
}
