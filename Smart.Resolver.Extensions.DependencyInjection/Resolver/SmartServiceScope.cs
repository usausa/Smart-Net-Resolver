namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

internal sealed class SmartServiceScope : IServiceScope, IKeyedServiceProvider
{
    private readonly SmartChildResolver childResolver;

    public IServiceProvider ServiceProvider => this;

    public SmartServiceScope(SmartResolver resolver)
    {
        childResolver = resolver.CreateChildResolver();
    }

    public void Dispose()
    {
        childResolver.Dispose();
    }

    public object GetService(Type serviceType)
    {
        return childResolver.Get(serviceType);
    }

    public object? GetKeyedService(Type serviceType, object? serviceKey)
    {
        return childResolver.TryGet(serviceType, serviceKey, out var obj) ? obj : null;
    }

    public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        return childResolver.Get(serviceType, serviceKey);
    }
}
