namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Components;

    public sealed class ContainerScope : IScope
    {
        private readonly int index;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
        public ContainerScope()
        {
            index = ContainerIndexManager.Acquire();
        }

        public IScope Copy(ComponentContainer components)
        {
            return new ContainerScope();
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
