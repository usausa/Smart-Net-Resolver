namespace Smart.Resolver.Bindings
{
    using System;

    public interface IBindingTable
    {
        IBinding[] FindBindings(Type type);
    }
}
