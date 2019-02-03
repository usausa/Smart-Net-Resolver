namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    public sealed class SmartServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceProvider serviceProvider;

        public SmartServiceScopeFactory(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public IServiceScope CreateScope()
        {
            return new SmartServiceScope(serviceProvider);
        }
    }
}
