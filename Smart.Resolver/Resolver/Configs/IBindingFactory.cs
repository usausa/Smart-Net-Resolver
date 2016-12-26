namespace Smart.Resolver.Configs
{
    using Smart.ComponentModel;

    using Smart.Resolver.Bindings;

    public interface IBindingFactory
    {
        IBinding CreateBinding(IComponentContainer components);
    }
}
