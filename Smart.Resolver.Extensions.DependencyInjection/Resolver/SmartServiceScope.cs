namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    internal sealed class SmartServiceScope : IServiceScope
    {
        private readonly IResolver childResolver;

        public IServiceProvider ServiceProvider { get; }

        public SmartServiceScope(SmartResolver resolver)
        {
            childResolver = resolver.CreateChildResolver();
            ServiceProvider = new SmartChildServiceProvider(childResolver);
        }

        public void Dispose()
        {
            childResolver.Dispose();
        }

        private sealed class SmartChildServiceProvider : IServiceProvider
        {
            private readonly IResolver resolver;

            public SmartChildServiceProvider(IResolver resolver)
            {
                this.resolver = resolver;
            }

            public object GetService(Type serviceType)
            {
                return resolver.Get(serviceType);
            }
        }
    }
}
