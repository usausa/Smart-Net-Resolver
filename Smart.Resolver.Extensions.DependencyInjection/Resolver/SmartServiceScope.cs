namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    public sealed class SmartServiceScope : IServiceScope
    {
        private readonly IResolver resolver;

        public IServiceProvider ServiceProvider { get; }

        public SmartServiceScope(IResolver resolver)
        {
            this.resolver = resolver;
            ServiceProvider = new SmartChildServiceProvider(resolver);
        }

        public void Dispose()
        {
            resolver.Dispose();
        }
    }
}
