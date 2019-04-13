namespace Smart.Resolver.Providers
{
    using System;

    using Smart.Resolver.Bindings;

    public interface IProvider
    {
        Type TargetType { get; }

        Func<IKernel, object> CreateFactory(IKernel kernel, IBinding binding);
    }
}
