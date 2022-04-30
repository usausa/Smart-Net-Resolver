namespace Smart.Resolver.Handlers;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;

public interface IMissingHandler
{
    IEnumerable<Binding> Handle(ComponentContainer components, BindingTable table, Type type);
}
