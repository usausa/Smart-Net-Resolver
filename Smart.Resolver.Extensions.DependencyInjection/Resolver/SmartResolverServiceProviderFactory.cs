namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    public sealed class SmartResolverServiceProviderFactory : IServiceProviderFactory<ResolverConfig>
    {
        private readonly ResolverConfig config;

        public SmartResolverServiceProviderFactory()
            : this(new ResolverConfig())
        {
        }

        public SmartResolverServiceProviderFactory(ResolverConfig config)
        {
            this.config = config;
        }

        public SmartResolverServiceProviderFactory(Action<ResolverConfig> action)
        {
            config = new ResolverConfig();
            action(config);
        }

        public ResolverConfig CreateBuilder(IServiceCollection services)
        {
            config.Populate(services);

            config.Bind<IServiceProvider>().To<SmartResolverServiceProvider>().InSingletonScope();
            config.Bind<IServiceScopeFactory>().To<SmartResolverServiceScopeFactory>().InSingletonScope();

            config.UseOpenGenericBinding();
            config.UseArrayBinding();

            return config;
        }

        public IServiceProvider CreateServiceProvider(ResolverConfig containerBuilder)
        {
            return new SmartResolverServiceProvider(containerBuilder.ToResolver());
        }
    }
}
