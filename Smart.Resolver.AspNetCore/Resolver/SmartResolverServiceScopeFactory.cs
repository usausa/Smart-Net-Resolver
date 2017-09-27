namespace Smart.Resolver
{
    using System;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public class SmartResolverServiceScopeFactory : IServiceScopeFactory
    {
        private readonly IServiceProvider serviceProvider;

        private readonly IHttpContextAccessor accessor;

        public SmartResolverServiceScopeFactory(IServiceProvider serviceProvider, IHttpContextAccessor accessor)
        {
            this.serviceProvider = serviceProvider;
            this.accessor = accessor;
        }

        public IServiceScope CreateScope()
        {
            return new SmartResolverServiceScope(serviceProvider, accessor);
        }
    }
}
