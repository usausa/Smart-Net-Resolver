namespace Smart.Resolver;

using System.Diagnostics.CodeAnalysis;

using Smart.ComponentModel;
using Smart.Reflection;
using Smart.Resolver.Bindings;
using Smart.Resolver.Builders;
using Smart.Resolver.Components;
using Smart.Resolver.Expressions;
using Smart.Resolver.Keys;

public sealed class ResolverConfig : IResolverConfig, IBindingRoot
{
    public ComponentConfig Components { get; } = new();

    private readonly List<IBindingFactory> bindingFactories = [];

    [UnconditionalSuppressMessage("AOT", "IL3050", Justification = "EmitFactoryBuilder is only registered when IsCodegenRequired is true, which means dynamic code is supported at runtime.")]
    public ResolverConfig()
    {
        Components.Add<DisposableStorage>();
        Components.Add<IKeySource, ResolveBySource>();
        Components.Add<IDelegateFactory>(DelegateFactory.Default);
        if (DelegateFactory.Default.IsCodegenRequired)
        {
            Components.Add<IFactoryBuilder, EmitFactoryBuilder>();
        }
        else
        {
            Components.Add<IFactoryBuilder, ReflectionFactoryBuilder>();
        }
    }

    //--------------------------------------------------------------------------------
    // IResolverConfig
    //--------------------------------------------------------------------------------

    ComponentContainer IResolverConfig.CreateComponentContainer()
    {
        return Components.ToContainer();
    }

    IEnumerable<Binding> IResolverConfig.CreateBindings(ComponentContainer components)
    {
        return bindingFactories.Select(f => f.CreateBinding(components));
    }

    //--------------------------------------------------------------------------------
    // IBindingRoot
    //--------------------------------------------------------------------------------

    public IBindingToSyntax<T> Bind<T>()
    {
        var builder = new BindingBuilder<T>(typeof(T));
        bindingFactories.Add(builder);
        return builder;
    }

    public IBindingToSyntax<object> Bind(
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] Type type)
    {
        var builder = new BindingBuilder<object>(type);
        bindingFactories.Add(builder);
        return builder;
    }
}
