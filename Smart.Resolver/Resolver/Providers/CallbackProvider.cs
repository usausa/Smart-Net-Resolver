namespace Smart.Resolver.Providers;

using System;

using Smart.Resolver.Bindings;

public sealed class CallbackProvider<T> : IProvider
    where T : class
{
    private readonly Func<IResolver, T> factory;

    public Type TargetType { get; }

    public CallbackProvider(Func<IResolver, T> factory)
    {
        this.factory = factory;
        TargetType = typeof(T);
    }

    public Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding)
    {
        return factory;
    }
}

public sealed class StructCallbackProvider<T> : IProvider
    where T : struct
{
    private readonly Func<IResolver, T> factory;

    public Type TargetType { get; }

    public StructCallbackProvider(Func<IResolver, T> factory)
    {
        this.factory = factory;
        TargetType = typeof(T);
    }

    public Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding)
    {
        return r => factory(r);
    }
}
