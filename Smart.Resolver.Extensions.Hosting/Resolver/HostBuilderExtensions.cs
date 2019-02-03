namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.Hosting;

    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseSmartResolver(this IHostBuilder builder)
        {
            return builder.UseServiceProviderFactory(new SmartResolverServiceProviderFactory());
        }

        public static IHostBuilder UseSmartResolver(this IHostBuilder builder, ResolverConfig config)
        {
            return builder.UseServiceProviderFactory(new SmartResolverServiceProviderFactory(config));
        }

        public static IHostBuilder UseSmartResolver(this IHostBuilder builder, Action<ResolverConfig> action)
        {
            return builder.UseServiceProviderFactory(new SmartResolverServiceProviderFactory(action));
        }
    }
}
