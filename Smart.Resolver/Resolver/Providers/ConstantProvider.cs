namespace Smart.Resolver.Providers;

using System.Diagnostics.CodeAnalysis;

using Smart.Resolver.Bindings;

public sealed class ConstantProvider<T> : IProvider, IConstantSource
{
    private readonly object value;

    public Type TargetType { get; }

    public DisposalTracking DisposalTracking => DisposalTracking.Never;

    object IConstantSource.Value => value;

    public ConstantProvider([DisallowNull] T value)
    {
        this.value = value;
        TargetType = typeof(T);
    }

    public Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding, object? key)
    {
        return _ => value;
    }
}
