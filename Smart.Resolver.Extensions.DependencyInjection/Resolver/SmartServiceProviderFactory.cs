namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

using Smart.Resolver.Keys;

public sealed class SmartServiceProviderFactory : IServiceProviderFactory<ResolverConfig>
{
    private readonly ResolverConfig config;

    public SmartServiceProviderFactory()
        : this(new ResolverConfig(), _ => { })
    {
    }

    public SmartServiceProviderFactory(ResolverConfig config)
        : this(config, _ => { })
    {
    }

    public SmartServiceProviderFactory(Action<ResolverConfig> action)
        : this(new ResolverConfig(), action)
    {
    }

    private SmartServiceProviderFactory(ResolverConfig config, Action<ResolverConfig> action)
    {
        this.config = config;
        action(config);
        config.Components.Add<IKeySource, FromKeyedServicesSource>();
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
        return new SmartServiceProvider(containerBuilder.ToResolver());
    }
}
