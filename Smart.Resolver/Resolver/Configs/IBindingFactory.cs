namespace Smart.Resolver.Configs
{
    using Smart.Resolver.Bindings;

    public interface IBindingFactory
    {
        IBinding CreateBinding();
    }
}
