namespace Smart.Resolver.Configs
{
    using Smart.ComponentModel;

    using Smart.Resolver.Bindings;

    public interface IBindingFactory
    {
        Binding CreateBinding(IComponentContainer components);
    }
}
