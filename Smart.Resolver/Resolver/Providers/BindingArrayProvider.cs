namespace Smart.Resolver.Providers;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;
using Smart.Resolver.Builders;

internal sealed class BindingArrayProvider : IProvider
{
    private readonly Type elementType;

    private readonly IFactoryBuilder builder;

    public Type TargetType { get; }

    public BindingArrayProvider(Type type, Type elementType, ComponentContainer components)
    {
        TargetType = type;
        this.elementType = elementType;
        builder = components.Get<IFactoryBuilder>();
    }

    public Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding, object? key)
    {
        kernel.TryResolveFactories(elementType, key, out var factories);
        return builder.CreateArrayFactory(elementType, factories);
    }
}
