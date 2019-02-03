namespace Smart
{
    using Microsoft.Extensions.DependencyInjection;

    using Smart.Resolver;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSmartResolver(this IServiceCollection services)
        {
            return services.AddSingleton<IServiceProviderFactory<ResolverConfig>>(new SmartResolverServiceProviderFactory());
        }
    }
}
