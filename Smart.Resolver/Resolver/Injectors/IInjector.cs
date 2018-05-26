namespace Smart.Resolver.Injectors
{
    using System;

    using Smart.Resolver.Bindings;

    public interface IInjector
    {
        bool IsTarget(Type type);

        void Initialize(IBinding binding, object instance);
    }
}
