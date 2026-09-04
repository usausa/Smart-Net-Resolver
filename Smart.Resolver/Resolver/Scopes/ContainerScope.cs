namespace Smart.Resolver.Scopes;

using Smart.ComponentModel;
using Smart.Resolver.Components;

public sealed class ContainerScope : IScope
{
    private readonly int index = ContainerIndexManager.Acquire();

    public bool TransferDisposal() => false;

    public IScope Copy(ComponentContainer components)
    {
        return new ContainerScope();
    }

    public Func<IResolver, object> Create(IResolver resolver, Func<IResolver, object> factory)
    {
        var adaptor = new ContainerAdaptor(index, factory);
        return adaptor.Resolve;
    }

    private sealed class ContainerAdaptor
    {
        private readonly int index;

        private readonly Func<IResolver, object> factory;

        public ContainerAdaptor(int index, Func<IResolver, object> factory)
        {
            this.index = index;
            this.factory = factory;
        }

        public object Resolve(IResolver resolver)
        {
            if (resolver is IContainer container)
            {
                return container.Slot.GetOrCreate(index, resolver, factory);
            }

            return factory(resolver);
        }
    }
}
