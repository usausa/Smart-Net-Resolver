namespace Smart.Resolver;

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

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

    public ResolverConfig()
    {
        Components.Add<DisposableStorage>();
        Components.Add<IKeySource, ResolveBySource>();
        Components.Add<IDelegateFactory>(DelegateFactory.Default);
        if (RuntimeFeature.IsDynamicCodeSupported && DelegateFactory.Default.IsCodegenRequired)
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

    public IBindingToSyntax<T> Bind<[DynamicallyAccessedMembers(LinkerFlags.Binding)] T>()
    {
        var builder = new BindingBuilder<T>(typeof(T));
        bindingFactories.Add(builder);
        return builder;
    }

    public IBindingToSyntax<object> Bind(
        [DynamicallyAccessedMembers(LinkerFlags.Binding)] Type type)
    {
        var builder = new BindingBuilder<object>(type);
        bindingFactories.Add(builder);
        return builder;
    }
}
