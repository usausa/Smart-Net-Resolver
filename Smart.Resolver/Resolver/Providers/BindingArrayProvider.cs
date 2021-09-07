namespace Smart.Resolver.Providers
{
    using System;
    using System.Linq;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Builders;

    internal sealed class BindingArrayProvider : IProvider
    {
        private readonly Type elementType;

        private readonly IFactoryBuilder builder;

        private readonly Binding[] bindings;

        public Type TargetType { get; }

        public BindingArrayProvider(Type type, Type elementType, ComponentContainer components, Binding[] bindings)
        {
            TargetType = type;
            this.elementType = elementType;
            builder = components.Get<IFactoryBuilder>();
            this.bindings = bindings;
        }

        public Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding)
        {
            var factories = bindings
                .Select(b => b.Provider.CreateFactory(kernel, b))
                .ToArray();
            return builder.CreateArrayFactory(elementType, factories);
        }
    }
}
