namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

using Smart.Resolver.Bindings;
using Smart.Resolver.Providers;

internal sealed class KeyedFactoryProvider : IProvider
{
    private readonly Func<IServiceProvider, object?, object> factory;

    private readonly object serviceKey;

    public Type TargetType { get; }

    public DisposalTracking DisposalTracking => DisposalTracking.Runtime;

    public KeyedFactoryProvider(Type serviceType, Func<IServiceProvider, object?, object> factory, object serviceKey)
    {
        TargetType = serviceType;
        this.factory = factory;
        this.serviceKey = serviceKey;
    }

    public Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding, object? key)
    {
        var resolvedKey = ReferenceEquals(serviceKey, KeyedService.AnyKey) ? key : serviceKey;
        return resolver => factory(resolver.Get<IServiceProvider>(), resolvedKey);
    }
}
