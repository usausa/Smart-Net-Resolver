namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    public class SmartResolverServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceProvider serviceProvider;

        private readonly RequestScopeStorage storage;

        public SmartResolverServiceScopeFactory(IServiceProvider serviceProvider, RequestScopeStorage storage)
        {
            this.serviceProvider = serviceProvider;
            this.storage = storage;
        }

        public IServiceScope CreateScope()
        {
            return new SmartResolverServiceScope(serviceProvider, storage);
        }
    }
}
