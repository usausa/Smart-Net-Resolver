namespace Smart.Resolver
{
    using System;

    public sealed class SmartServiceProvider : IServiceProvider, IDisposable
    {
        private readonly SmartResolver resolver;

        public SmartServiceProvider(SmartResolver resolver)
        {
            this.resolver = resolver;
        }

        public object GetService(Type serviceType)
        {
            return resolver.Get(serviceType);
        }

        public void Dispose()
        {
            resolver.Dispose();
        }
    }
}
