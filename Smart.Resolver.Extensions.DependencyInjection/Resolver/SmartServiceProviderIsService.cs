namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

#pragma warning disable CA1812
internal sealed class SmartServiceProviderIsService : IServiceProviderIsKeyedService
{
    private readonly IResolver resolver;

    public SmartServiceProviderIsService(IResolver resolver)
    {
        this.resolver = resolver;
    }

    public bool IsService(Type serviceType) => !serviceType.ContainsGenericParameters && resolver.CanGet(serviceType);

    public bool IsKeyedService(Type serviceType, object? serviceKey)
    {
        if (serviceKey is null)
        {
            return IsService(serviceType);
        }

        return !serviceType.ContainsGenericParameters && resolver.CanGet(serviceType, serviceKey);
    }
}
#pragma warning restore CA1812
