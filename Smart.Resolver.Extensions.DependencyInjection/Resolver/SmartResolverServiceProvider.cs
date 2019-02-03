namespace Smart.Resolver
{
    using System;

    public sealed class SmartResolverServiceProvider : IServiceProvider
    {
        private readonly SmartResolver resolver;

        public SmartResolverServiceProvider(SmartResolver resolver)
        {
            this.resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            return resolver.Get(serviceType);
        }
    }
}
