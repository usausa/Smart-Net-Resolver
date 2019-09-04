namespace Smart.Resolver
{
    using Microsoft.Extensions.DependencyInjection;

    public sealed class SmartServiceScopeFactory : IServiceScopeFactory
    {
        private readonly SmartResolver resolver;

        public SmartServiceScopeFactory(SmartResolver resolver)
        {
            this.resolver = resolver;
        }

        public IServiceScope CreateScope()
        {
            return new SmartServiceScope(resolver);
        }
    }
}
