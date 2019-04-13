namespace Smart.Resolver.Injectors
{
    using System;

    using Smart.Resolver.Bindings;

    public interface IInjector
    {
        Action<IResolver, object> CreateInjector(Type type, IBinding binding);
    }
}
