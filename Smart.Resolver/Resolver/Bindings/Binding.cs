namespace Smart.Resolver.Bindings
{
    using System;

    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    public sealed class Binding : IBinding
    {
        private static readonly IBindingMetadata EmptyBindingMetadata = new BindingMetadata();

        private static readonly ParameterMap EmptyPropertyMap = new ParameterMap(null);

        public Type Type { get; }

        public IProvider Provider { get; }

        public IScope Scope { get; }

        public IBindingMetadata Metadata { get; }

        public ParameterMap ConstructorArguments { get; }

        public ParameterMap PropertyValues { get; }

        public Binding(Type type)
            : this(type, null, null, null, null, null)
        {
        }

        public Binding(Type type, IProvider provider)
            : this(type, provider, null, null, null, null)
        {
        }

        public Binding(Type type, IProvider provider, IScope scope, IBindingMetadata metadata, ParameterMap constructorArguments, ParameterMap propertyValues)
        {
            Type = type;
            Provider = provider;
            Scope = scope;
            Metadata = metadata ?? EmptyBindingMetadata;
            ConstructorArguments = constructorArguments ?? EmptyPropertyMap;
            PropertyValues = propertyValues ?? EmptyPropertyMap;
        }
    }
}
