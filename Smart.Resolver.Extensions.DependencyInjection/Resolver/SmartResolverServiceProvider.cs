namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

internal sealed class SmartResolverServiceProvider : IKeyedServiceProvider
{
    private readonly IResolver resolver;

    public SmartResolverServiceProvider(IResolver resolver)
    {
        this.resolver = resolver;
    }

    public object? GetService(Type serviceType)
    {
        return resolver.TryGet(serviceType, out var obj) ? obj : null;
    }

    public object? GetKeyedService(Type serviceType, object? serviceKey)
    {
        if (serviceKey is null)
        {
            return resolver.TryGet(serviceType, out var obj) ? obj : null;
        }

        if (ReferenceEquals(serviceKey, KeyedService.AnyKey) && !ServiceTypeHelper.IsEnumerableService(serviceType))
        {
            ThrowHelper.ThrowAnyKeyNotSupported();
        }

        return resolver.TryGet(serviceType, serviceKey, out var keyed) ? keyed : null;
    }

    public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
    {
        if (serviceKey is null)
        {
            if (!resolver.TryGet(serviceType, out var obj))
            {
                ThrowHelper.ThrowServiceNotRegistered(serviceType);
            }

            return obj;
        }

        if (ReferenceEquals(serviceKey, KeyedService.AnyKey) && !ServiceTypeHelper.IsEnumerableService(serviceType))
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
