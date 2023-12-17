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
