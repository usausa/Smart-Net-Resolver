namespace Smart.Resolver;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;

public interface IResolverConfig
{
    ComponentContainer CreateComponentContainer();

    IEnumerable<Binding> CreateBindings(ComponentContainer components);
}
