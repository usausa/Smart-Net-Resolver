namespace Smart.Resolver
{
    using System;

    using Smart.Resolver.Bindings;

    public interface IContainer
    {
        object Create(IBinding binding, Func<object> factory);
    }
}
