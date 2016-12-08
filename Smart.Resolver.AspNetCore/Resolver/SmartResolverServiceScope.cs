namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    public class SmartResolverServiceScope : IServiceScope
    {
        private readonly RequestScopeStorage storage;

        public IServiceProvider ServiceProvider { get; }

        public SmartResolverServiceScope(IServiceProvider serviceProvider, RequestScopeStorage storage)
        {
            ServiceProvider = serviceProvider;
            this.storage = storage;
        }

        public void Dispose()
        {
            storage.Clear();
        }
    }
}
