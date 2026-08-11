namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

using Smart.Resolver.Constraints;
using Smart.Resolver.Expressions;

public static class ResolverConfigExtensions
{
    public static void Populate(this ResolverConfig config, IEnumerable<ServiceDescriptor> descriptors)
    {
        foreach (var descriptor in descriptors)
        {
            if (descriptor.IsKeyedService)
            {
                var constraint = ReferenceEquals(descriptor.ServiceKey, KeyedService.AnyKey)
                    ? MediAnyKeyConstraint.Instance
                    : (IConstraint)new MediKeyConstraint(descriptor.ServiceKey!);

                if (descriptor.KeyedImplementationType is not null)
                {
                    config
                        .Bind(descriptor.ServiceType)
                        .To(descriptor.KeyedImplementationType)
                        .ConfigureScope(descriptor.Lifetime)
                        .Constraint(constraint);
                }
                else if (descriptor.KeyedImplementationFactory is not null)
                {
                    config
                        .Bind(descriptor.ServiceType)
                        .ToProvider(_ => new KeyedFactoryProvider(descriptor.ServiceType, descriptor.KeyedImplementationFactory, descriptor.ServiceKey!))
                        .ConfigureScope(descriptor.Lifetime)
                        .Constraint(constraint);
                }
                else if (descriptor.KeyedImplementationInstance is not null)
                {
                    config
                        .Bind(descriptor.ServiceType)
                        .ToConstant(descriptor.KeyedImplementationInstance)
                        .ConfigureScope(descriptor.Lifetime)
                        .Constraint(constraint);
                }
            }
            else
            {
                if (descriptor.ImplementationType is not null)
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
