namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Components;

    public sealed class ContainerScope : IScope
    {
        private readonly int index;

        public ContainerScope(IComponentContainer components)
        {
            index = components.Get<ContainerIndexManager>().Acquire();
        }

        public IScope Copy(IComponentContainer components)
        {
            return this;
        }

        public Func<IResolver, object> Create(Func<object> factory)
        {
            return resolver =>
            {
                if (resolver is IContainer container)
                {
                    return container.Slot.GetOrCreate(index, factory);
                }

                return factory();
            };
        }
    }
}
