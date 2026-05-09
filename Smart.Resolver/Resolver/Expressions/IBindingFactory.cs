namespace Smart.Resolver.Expressions;

using Smart.ComponentModel;

using Smart.Resolver.Bindings;

public interface IBindingFactory
{
    Binding CreateBinding(ComponentContainer components);
}
