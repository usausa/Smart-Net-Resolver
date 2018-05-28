namespace Smart.Resolver.Injectors
{
    using System;

    using Smart.Resolver.Bindings;

    public interface IInjector
    {
        Action<object> CreateInjector(Type type, IKernel kernel, IBinding binding);
    }
}
