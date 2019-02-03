namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    public sealed class SmartResolverServiceScope : IServiceScope
    {
        public IServiceProvider ServiceProvider { get; }

        public SmartResolverServiceScope(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            AsyncContext.Initializable();
        }

        public void Dispose()
        {
            AsyncContext.Clear();
        }
    }
}
