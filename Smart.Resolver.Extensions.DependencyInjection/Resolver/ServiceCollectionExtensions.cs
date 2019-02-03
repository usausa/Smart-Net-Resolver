namespace Smart.Resolver
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmartResolver(this IServiceCollection services)
        {
            return services.AddSingleton<IServiceProviderFactory<ResolverConfig>>(new SmartResolverServiceProviderFactory());
        }
    }
}
