namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    public sealed class ContainerScope : IScope
    {
        public IScope Copy(IComponentContainer components)
        {
            return this;
        }

        public Func<IResolver, object> Create(IBinding binding, Func<object> factory)
        {
            return resolver =>
            {
                if (resolver is IContainer container)
                {
                    return container.Create(binding, factory);
                }

                return factory();
            };
        }
    }
}
