namespace Smart.Resolver.Providers;

using System.Diagnostics.CodeAnalysis;

using Smart.Resolver.Bindings;

public sealed class ConstantProvider<T> : IProvider, IConstantSource
{
    private readonly object value;

    public Type TargetType { get; }

    object IConstantSource.Value => value;

    public ConstantProvider([DisallowNull] T value)
    {
        this.value = value;
        TargetType = typeof(T);
    }

    public Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding)
    {
        return _ => value;
    }
}
