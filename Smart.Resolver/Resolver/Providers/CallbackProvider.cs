namespace Smart.Resolver.Providers;

using Smart.Resolver.Bindings;

public sealed class CallbackProvider<T> : IProvider
{
    private readonly Func<IResolver, T> factory;

    public Type TargetType { get; }

    public DisposalTracking DisposalTracking { get; init; } = DisposalTracking.Runtime;

    public CallbackProvider(Func<IResolver, T> factory)
    {
        this.factory = factory;
        TargetType = typeof(T);
    }

    public Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding, object? key)
    {
        return factory as Func<IResolver, object> ?? (r => factory(r)!);
    }
}
