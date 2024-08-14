namespace Smart.Resolver.Providers;

using System.Diagnostics.CodeAnalysis;

using Smart.Resolver.Bindings;

// TODO constraint version ?
public sealed class ConstantProvider<T> : IProvider
{
    private readonly object value;

    public Type TargetType { get; }

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
