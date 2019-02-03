namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    public sealed class SmartServiceScope : IServiceScope
    {
        public IServiceProvider ServiceProvider { get; }

        public SmartServiceScope(IServiceProvider serviceProvider)
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
