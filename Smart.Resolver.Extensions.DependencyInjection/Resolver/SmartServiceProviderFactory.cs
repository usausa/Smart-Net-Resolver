namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.DependencyInjection;

    public sealed class SmartServiceProviderFactory : IServiceProviderFactory<ResolverConfig>
    {
        private readonly ResolverConfig config;

        public SmartServiceProviderFactory()
            : this(new ResolverConfig())
        {
        }

        public SmartServiceProviderFactory(ResolverConfig config)
        {
            this.config = config;
        }

        public SmartServiceProviderFactory(Action<ResolverConfig> action)
        {
            if (action is null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            config = new ResolverConfig();
            action(config);
        }

        public ResolverConfig CreateBuilder(IServiceCollection services)
        {
            config.Populate(services);

            config.Bind<IServiceProvider>().To<SmartServiceProvider>().InSingletonScope();
            config.Bind<IServiceScopeFactory>().To<SmartServiceScopeFactory>().InSingletonScope();

            config.UseOpenGenericBinding();
            config.UseArrayBinding();

            return config;
        }

        public IServiceProvider CreateServiceProvider(ResolverConfig containerBuilder)
        {
            return new SmartServiceProvider(containerBuilder.ToResolver());
        }
    }
}
