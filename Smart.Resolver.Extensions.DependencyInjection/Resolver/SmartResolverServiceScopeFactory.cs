namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    public sealed class SmartResolverServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceProvider serviceProvider;

        public SmartResolverServiceScopeFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IServiceScope CreateScope()
        {
            return new SmartResolverServiceScope(serviceProvider);
        }
    }
}
