namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;

    using Smart.Resolver.Parameters;
    using Smart.Resolver.Providers;
    using Smart.Resolver.Scopes;

    public sealed class NullBinding : IBinding
    {
        private static readonly IBindingMetadata EmptyBindingMetadata = new BindingMetadata();

        private static readonly ParameterMap EmptyPropertyMap = new ParameterMap(new Dictionary<string, IParameter>());

        public Type Type { get; }

        public IBindingMetadata Metadata => EmptyBindingMetadata;

        public IProvider Provider => null;

        public IScope Scope => null;

        public ParameterMap ConstructorArguments => EmptyPropertyMap;

        public ParameterMap PropertyValues => EmptyPropertyMap;

        public NullBinding(Type type)
        {
            Type = type;
        }
    }
}
