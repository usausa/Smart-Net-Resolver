namespace Smart.Resolver
{
    using System;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.DependencyInjection;

    public class SmartResolverServiceScope : IServiceScope
    {
        private readonly IHttpContextAccessor accessor;

        public IServiceProvider ServiceProvider { get; }

        public SmartResolverServiceScope(IServiceProvider serviceProvider, IHttpContextAccessor accessor)
        {
            ServiceProvider = serviceProvider;
            this.accessor = accessor;
        }

        public void Dispose()
        {
            HttpContextStorage.Clear(accessor.HttpContext);
        }
    }
}
