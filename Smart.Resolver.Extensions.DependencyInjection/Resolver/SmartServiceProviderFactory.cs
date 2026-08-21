namespace Smart.Resolver;

using Microsoft.Extensions.DependencyInjection;

using Smart.Resolver.Keys;
using Smart.Resolver.Providers;

public sealed class SmartServiceProviderFactory : IServiceProviderFactory<ResolverConfig>
{
    private readonly ResolverConfig config;

    public SmartServiceProviderOption Option { get; init; } = new();

    public SmartServiceProviderFactory()
        : this(new ResolverConfig(), static _ => { })
    {
    }

    public SmartServiceProviderFactory(ResolverConfig config)
        : this(config, static _ => { })
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
        config.Components.Add<IKeySource, ServiceKeySource>();
    }

    public ResolverConfig CreateBuilder(IServiceCollection services)
    {
        config.Populate(services);

        config.Bind<IServiceScopeFactory>().To<SmartServiceScopeFactory>().InSingletonScope();
        config.Bind<IServiceProvider>().ToProvider(static _ => new CallbackProvider<IServiceProvider>(static r => new SmartResolverServiceProvider(r)) { DisposalTracking = DisposalTracking.Never }).InContainerScope();
        config.Bind<IServiceProviderIsService>().ToMethod(static r => new SmartServiceProviderIsService(r)).InSingletonScope();
        config.Bind<IServiceProviderIsKeyedService>().ToMethod(static r => new SmartServiceProviderIsService(r)).InSingletonScope();

        config.UseOpenGenericBinding();
        config.UseArrayBinding();
        config.UseOption(new ResolverOption { DisposalTracking = Option.DisposalTracking });

        return config;
    }

    public IServiceProvider CreateServiceProvider(ResolverConfig containerBuilder)
    {
        var resolver = containerBuilder.ToResolver();
        return Option.RootScope ? new SmartRootScopeServiceProvider(resolver) : new SmartServiceProvider(resolver);
    }
}
