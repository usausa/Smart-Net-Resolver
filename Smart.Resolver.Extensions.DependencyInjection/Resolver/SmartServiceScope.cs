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
        if (serviceKey is null)
        {
            return childResolver.Get(serviceType);
        }

        if (ReferenceEquals(serviceKey, KeyedService.AnyKey) && !KeyedServiceHelper.IsEnumerableService(serviceType))
        {
            ThrowHelper.ThrowAnyKeyNotSupported();
        }

        return childResolver.TryGet(serviceType, serviceKey, out var obj) ? obj : null;
    }

    public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        if (serviceKey is null)
        {
            var obj = childResolver.Get(serviceType);
            if (obj is null)
            {
                ThrowHelper.ThrowServiceNotRegistered(serviceType);
            }

            return obj;
        }

        if (ReferenceEquals(serviceKey, KeyedService.AnyKey) && !KeyedServiceHelper.IsEnumerableService(serviceType))
        {
            ThrowHelper.ThrowAnyKeyNotSupported();
        }

        if (!childResolver.TryGet(serviceType, serviceKey, out var keyed))
        {
            ThrowHelper.ThrowKeyedServiceNotRegistered(serviceType, serviceKey);
        }

        return keyed;
    }
}
