namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    using Smart.Resolver.Bindings;

    public static class SmartResolverHelper
    {
        public static IServiceProvider BuildServiceProvider(StandardResolver resolver, IEnumerable<ServiceDescriptor> descriptors)
        {
            foreach (var descriptor in descriptors)
            {
                if (descriptor.ImplementationType != null)
                {
                    resolver
                        .Bind(descriptor.ServiceType)
                        .To(descriptor.ImplementationType)
                        .ConfigureScope(descriptor.Lifetime);
                }
                else if (descriptor.ImplementationFactory != null)
                {
                    resolver
                        .Bind(descriptor.ServiceType)
                        .ToMethod(kernel => descriptor.ImplementationFactory(kernel.Get<IServiceProvider>()))
                        .ConfigureScope(descriptor.Lifetime);
                }
                else if (descriptor.ImplementationInstance != null)
                {
                    resolver
                        .Bind(descriptor.ServiceType)
                        .ToConstant(descriptor.ImplementationInstance)
                        .ConfigureScope(descriptor.Lifetime);
                }
            }

            resolver.Bind<IServiceProvider>().To<SmartResolverServiceProvider>().InSingletonScope();
            resolver.Bind<IServiceScopeFactory>().To<SmartResolverServiceScopeFactory>().InSingletonScope();
            resolver.Bind<IHttpContextAccessor>().To<HttpContextAccessor>().InSingletonScope();
            resolver.Bind<RequestScopeStorage>().ToSelf().InSingletonScope();

            return resolver.Get<IServiceProvider>();
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
                    syntax.InRequestScope();
                    break;
            }
        }
    }
}
