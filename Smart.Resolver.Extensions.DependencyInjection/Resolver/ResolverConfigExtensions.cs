namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Microsoft.Extensions.DependencyInjection;

    using Smart.Resolver.Configs;

    public static class ResolverConfigExtensions
    {
        public static void Populate(this ResolverConfig config, IEnumerable<ServiceDescriptor> descriptors)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (descriptors is null)
            {
                throw new ArgumentNullException(nameof(descriptors));
            }

            foreach (var descriptor in descriptors)
            {
                if (descriptor.ImplementationType != null)
                {
                    config
                        .Bind(descriptor.ServiceType)
                        .To(descriptor.ImplementationType)
                        .ConfigureScope(descriptor.Lifetime);
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    config
                        .Bind(descriptor.ServiceType)
                        .ToMethod(kernel => descriptor.ImplementationFactory(kernel.Get<IServiceProvider>()))
                        .ConfigureScope(descriptor.Lifetime);
                }
                else if (descriptor.ImplementationInstance != null)
                {
                    config
                        .Bind(descriptor.ServiceType)
                        .ToConstant(descriptor.ImplementationInstance)
                        .ConfigureScope(descriptor.Lifetime);
                }
            }
        }

        private static void ConfigureScope(this IBindingInSyntax syntax, ServiceLifetime lifetime)
        {
            switch (lifetime)
            {
                case ServiceLifetime.Singleton:
                    syntax.InSingletonScope();
                    break;
                case ServiceLifetime.Transient:
                    syntax.InTransientScope();
                    break;
                case ServiceLifetime.Scoped:
                    syntax.InContainerScope();
                    break;
            }
        }
    }
}
