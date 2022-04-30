namespace Smart.Resolver;

using Smart.ComponentModel;
using Smart.Reflection;
using Smart.Resolver.Bindings;
using Smart.Resolver.Builders;
using Smart.Resolver.Components;
using Smart.Resolver.Configs;

public class ResolverConfig : IResolverConfig, IBindingRoot
{
    public ComponentConfig Components { get; } = new();

    private readonly List<IBindingFactory> bindingFactories = new();

    public ResolverConfig()
    {
        Components.Add<DisposableStorage>();
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

    // ------------------------------------------------------------
    // IResolverConfig
    // ------------------------------------------------------------

    ComponentContainer IResolverConfig.CreateComponentContainer()
    {
        return CreateComponentContainer();
    }

    protected virtual ComponentContainer CreateComponentContainer()
    {
        return Components.ToContainer();
    }

    IEnumerable<Binding> IResolverConfig.CreateBindings(ComponentContainer components)
    {
        return CreateBindings(components);
    }

    protected virtual IEnumerable<Binding> CreateBindings(ComponentContainer components)
    {
        return bindingFactories.Select(f => f.CreateBinding(components));
    }

    // ------------------------------------------------------------
    // IBindingRoot
    // ------------------------------------------------------------

    public IBindingToSyntax<T> Bind<T>()
    {
        var builder = new BindingBuilder<T>(typeof(T));
        bindingFactories.Add(builder);
        return builder;
    }

    public IBindingToSyntax<object> Bind(Type type)
    {
        var builder = new BindingBuilder<object>(type);
        bindingFactories.Add(builder);
        return builder;
    }
}
