namespace Smart.Resolver
{
    using System;

    public sealed class SmartResolverServiceProvider : IServiceProvider
    {
        private readonly SmartResolver resolver;

        /// <summary>
        ///
        /// </summary>
        /// <param name="resolver"></param>
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
