namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

using Smart.Resolver.Configs;

public static class ResolverConfigExtensions
{
    public static void Populate(this ResolverConfig config, IEnumerable<ServiceDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            if (descriptor.KeyedImplementationType is not null)
            {
                config
                    .Bind(descriptor.ServiceType)
                    .To(descriptor.KeyedImplementationType)
                    .ConfigureScope(descriptor.Lifetime)
                    .Keyed(descriptor.ServiceKey!);
            }
            else if (descriptor.KeyedImplementationFactory is not null)
            {
                config
                    .Bind(descriptor.ServiceType)
                    .ToMethod(kernel => descriptor.KeyedImplementationFactory(kernel.Get<IServiceProvider>(), descriptor.ServiceKey))
                    .ConfigureScope(descriptor.Lifetime)
                    .Keyed(descriptor.ServiceKey!);
            }
            else if (descriptor.KeyedImplementationInstance is not null)
            {
                config
                    .Bind(descriptor.ServiceType)
                    .ToConstant(descriptor.KeyedImplementationInstance)
                    .ConfigureScope(descriptor.Lifetime)
                    .Keyed(descriptor.ServiceKey!);
            }
            else if (descriptor.ImplementationType is not null)
            {
                config
                    .Bind(descriptor.ServiceType)
                    .To(descriptor.ImplementationType)
                    .ConfigureScope(descriptor.Lifetime);
            }
            else if (descriptor.ImplementationFactory is not null)
            {
                config
                    .Bind(descriptor.ServiceType)
                    .ToMethod(kernel => descriptor.ImplementationFactory(kernel.Get<IServiceProvider>()))
                    .ConfigureScope(descriptor.Lifetime);
            }
            else if (descriptor.ImplementationInstance is not null)
            {
                config
                    .Bind(descriptor.ServiceType)
                    .ToConstant(descriptor.ImplementationInstance)
                    .ConfigureScope(descriptor.Lifetime);
            }
        }
    }

    private static IBindingConstraintWithSyntax ConfigureScope(this IBindingInSyntax syntax, ServiceLifetime lifetime)
    {
        return lifetime switch
        {
            ServiceLifetime.Transient => syntax.InTransientScope(),
            ServiceLifetime.Scoped => syntax.InContainerScope(),
            _ => syntax.InSingletonScope()
        };
    }
}
