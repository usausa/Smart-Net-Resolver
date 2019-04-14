namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    internal sealed class SmartServiceScope : IServiceScope
    {
        private readonly IResolver resolver;

        public IServiceProvider ServiceProvider { get; }

        public SmartServiceScope(SmartResolver resolver)
        {
            this.resolver = resolver;
            ServiceProvider = new SmartChildServiceProvider(resolver.CreateChildResolver());
        }

        public void Dispose()
        {
            resolver.Dispose();
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
