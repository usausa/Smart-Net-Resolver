namespace Smart.Resolver;

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
        config = new ResolverConfig();
        action(config);
    }

    public ResolverConfig CreateBuilder(IServiceCollection services)
    {
        config.Populate(services);

        config.Bind<IServiceScopeFactory>().To<SmartServiceScopeFactory>().InSingletonScope();

        config.UseOpenGenericBinding();
        config.UseArrayBinding();

        return config;
    }

    public IServiceProvider CreateServiceProvider(ResolverConfig containerBuilder)
    {
        return containerBuilder.ToResolver();
    }
}
