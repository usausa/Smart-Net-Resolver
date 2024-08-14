namespace Smart.Resolver.Scopes;

using Smart.ComponentModel;
using Smart.Resolver.Components;

public sealed class ContainerScope : IScope
{
    private readonly int index;

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
        var adaptor = new ContainerAdaptor(index, factory);
        return adaptor.Resolve;
    }

    private sealed class ContainerAdaptor
    {
        private readonly int index;

        private readonly Func<object> factory;

        public ContainerAdaptor(int index, Func<object> factory)
        {
            this.index = index;
            this.factory = factory;
        }

        public object Resolve(IResolver resolver)
        {
            if (resolver is IContainer container)
            {
                return container.Slot.GetOrCreate(index, factory);
            }

            return factory();
        }
    }
}
