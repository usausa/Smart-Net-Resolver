namespace Smart.Resolver.Bindings
{
    using System;

    public interface IBindingResolver
    {
        IBinding Resolve(Type type);
    }
}
