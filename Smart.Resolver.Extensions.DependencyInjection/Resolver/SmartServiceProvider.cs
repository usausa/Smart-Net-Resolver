namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

internal sealed class SmartServiceProvider : IKeyedServiceProvider
{
    private readonly SmartResolver resolver;

    public SmartServiceProvider(SmartResolver resolver)
    {
        this.resolver = resolver;
    }

    public object GetService(Type serviceType)
    {
        return resolver.Get(serviceType);
    }

    public object? GetKeyedService(Type serviceType, object? serviceKey)
    {
        return resolver.TryGet(serviceType, serviceKey, out var obj) ? obj : null;
    }

    public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        return resolver.Get(serviceType, serviceKey);
    }
}
