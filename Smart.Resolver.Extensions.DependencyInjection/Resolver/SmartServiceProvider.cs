namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

internal sealed class SmartServiceProvider : IKeyedServiceProvider, IDisposable
{
    private readonly SmartResolver resolver;

    public SmartServiceProvider(SmartResolver resolver)
    {
        this.resolver = resolver;
    }

    public void Dispose()
    {
        resolver.Dispose();
    }

    public object GetService(Type serviceType)
    {
        return resolver.Get(serviceType);
    }

    public object? GetKeyedService(Type serviceType, object? serviceKey)
    {
        if (serviceKey is null)
        {
            return resolver.Get(serviceType);
        }

        if (ReferenceEquals(serviceKey, KeyedService.AnyKey) && !KeyedServiceHelper.IsEnumerableService(serviceType))
        {
            ThrowHelper.ThrowAnyKeyNotSupported();
        }

        return resolver.TryGet(serviceType, serviceKey, out var obj) ? obj : null;
    }

    public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        if (serviceKey is null)
        {
            var obj = resolver.Get(serviceType);
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

        if (!resolver.TryGet(serviceType, serviceKey, out var keyed))
        {
            ThrowHelper.ThrowKeyedServiceNotRegistered(serviceType, serviceKey);
        }

        return keyed;
    }
}
