namespace Smart.Resolver.Configs;

using System.Diagnostics.CodeAnalysis;

using Smart.ComponentModel;
using Smart.Resolver.Bindings;
using Smart.Resolver.Parameters;
using Smart.Resolver.Providers;
using Smart.Resolver.Scopes;

public sealed class BindingBuilder<T> : IBindingFactory, IBindingToInNamedWithSyntax<T>
{
    private readonly Type targetType;

    private Func<ComponentContainer, IProvider> providerFactory = default!;

    private Func<ComponentContainer, IScope>? scopeFactory;

    private string? metadataName;

    private Dictionary<string, object?>? metadataValues;

    private Dictionary<string, Func<ComponentContainer, IParameter>>? constructorArgumentFactories;

    private Dictionary<string, Func<ComponentContainer, IParameter>>? propertyValueFactories;

    public BindingBuilder(Type type)
    {
        targetType = type;
    }

    // ------------------------------------------------------------
    // To
    // ------------------------------------------------------------

    public IBindingInNamedWithSyntax ToProvider(Func<ComponentContainer, IProvider> factory)
    {
        providerFactory = factory;
        return this;
    }

    public IBindingInNamedWithSyntax ToSelf()
    {
        return ToProvider(c => new StandardProvider(targetType, c));
    }

    public IBindingInNamedWithSyntax To<TImplementation>()
        where TImplementation : T
    {
        return ToProvider(static c => new StandardProvider(typeof(TImplementation), c));
    }

    public IBindingInNamedWithSyntax To(Type implementationType)
    {
        return ToProvider(c => new StandardProvider(implementationType, c));
    }

    public IBindingInNamedWithSyntax ToMethod(Func<IResolver, T> factory)
    {
        var genericType = typeof(T);
        var providerType = genericType.IsValueType
            ? typeof(StructCallbackProvider<>).MakeGenericType(genericType)
            : typeof(CallbackProvider<>).MakeGenericType(genericType);
        var provider = (IProvider)Activator.CreateInstance(providerType, factory)!;
        return ToProvider(_ => provider);
    }

    public IBindingInNamedWithSyntax ToConstant([DisallowNull] T value)
    {
        return ToProvider(_ => new ConstantProvider<T>(value));
    }

    // ------------------------------------------------------------
    // In
    // ------------------------------------------------------------

    public IBindingNamedWithSyntax InScope(Func<ComponentContainer, IScope> factory)
    {
        scopeFactory = factory;
        return this;
    }

    public IBindingNamedWithSyntax InTransientScope()
    {
        scopeFactory = null;
        return this;
    }

    public IBindingNamedWithSyntax InSingletonScope()
    {
        InScope(static c => new SingletonScope(c));
        return this;
    }

    public IBindingNamedWithSyntax InContainerScope()
    {
        InScope(static _ => new ContainerScope());
        return this;
    }

    // ------------------------------------------------------------
    // Named
    // ------------------------------------------------------------

    public IBindingWithSyntax Named(string name)
    {
        metadataName = name;
        return this;
    }

    // ------------------------------------------------------------
    // With
    // ------------------------------------------------------------

    public IBindingWithSyntax WithMetadata(string key, object? value)
    {
        metadataValues ??= [];
        metadataValues[key] = value;
        return this;
    }

    public IBindingWithSyntax WithConstructorArgument(string name, Func<ComponentContainer, IParameter> factory)
    {
        constructorArgumentFactories ??= [];
        constructorArgumentFactories[name] = factory;
        return this;
    }

    public IBindingWithSyntax WithConstructorArgument(string name, object? value)
    {
        WithConstructorArgument(name, _ => new ConstantParameter(value));
        return this;
    }

    public IBindingWithSyntax WithConstructorArgument(string name, Func<IResolver, object?> factory)
    {
        WithConstructorArgument(name, _ => new CallbackParameter(factory));
        return this;
    }

    public IBindingWithSyntax WithPropertyValue(string name, Func<ComponentContainer, IParameter> factory)
    {
        propertyValueFactories ??= [];
        propertyValueFactories[name] = factory;
        return this;
    }

    public IBindingWithSyntax WithPropertyValue(string name, object? value)
    {
        WithPropertyValue(name, _ => new ConstantParameter(value));
        return this;
    }

    public IBindingWithSyntax WithPropertyValue(string name, Func<IResolver, object?> factory)
    {
        WithPropertyValue(name, _ => new CallbackParameter(factory));
        return this;
    }

    // ------------------------------------------------------------
    // Factory
    // ------------------------------------------------------------

    Binding IBindingFactory.CreateBinding(ComponentContainer components)
    {
        return new(
            targetType,
            providerFactory.Invoke(components),
            scopeFactory?.Invoke(components),
            new BindingMetadata(metadataName, metadataValues),
            new ParameterMap(constructorArgumentFactories?.ToDictionary(static kv => kv.Key, kv => kv.Value(components))),
            new ParameterMap(propertyValueFactories?.ToDictionary(static kv => kv.Key, kv => kv.Value(components))));
    }
}
