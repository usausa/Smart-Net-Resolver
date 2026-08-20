namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

internal sealed class SmartRootScopeServiceProvider : IKeyedServiceProvider, IDisposable
{
    private readonly SmartResolver resolver;

    private readonly SmartChildResolver rootScope;

    public SmartRootScopeServiceProvider(SmartResolver resolver)
    {
        this.resolver = resolver;
        rootScope = resolver.CreateChildResolver();
    }

    public void Dispose()
    {
        rootScope.Dispose();
        resolver.Dispose();
    }

    public object GetService(Type serviceType)
    {
        return rootScope.Get(serviceType);
    }

    public object? GetKeyedService(Type serviceType, object? serviceKey)
    {
        if (serviceKey is null)
        {
            return rootScope.TryGet(serviceType, out var obj) ? obj : null;
        }

        if (ReferenceEquals(serviceKey, KeyedService.AnyKey) && !KeyedServiceHelper.IsEnumerableService(serviceType))
        {
            ThrowHelper.ThrowAnyKeyNotSupported();
        }

        return rootScope.TryGet(serviceType, serviceKey, out var keyed) ? keyed : null;
    }

    public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        if (serviceKey is null)
        {
            if (!rootScope.TryGet(serviceType, out var obj))
            {
                ThrowHelper.ThrowServiceNotRegistered(serviceType);
            }

            return obj;
        }

        if (ReferenceEquals(serviceKey, KeyedService.AnyKey) && !KeyedServiceHelper.IsEnumerableService(serviceType))
        {
            ThrowHelper.ThrowAnyKeyNotSupported();
        }

        if (!rootScope.TryGet(serviceType, serviceKey, out var keyed))
        {
            ThrowHelper.ThrowKeyedServiceNotRegistered(serviceType, serviceKey);
        }

        return keyed;
    }
}
