namespace Smart.Resolver
{
    using System;

    public sealed class SmartChildServiceProvider : IServiceProvider
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
