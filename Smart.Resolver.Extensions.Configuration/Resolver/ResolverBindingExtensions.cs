namespace Smart.Resolver
{
    using System;

    using Microsoft.Extensions.Configuration;

    public static class ResolverBindingExtensions
    {
        public static ResolverConfig BindConfig<TConfig>(
            this ResolverConfig config,
            IConfiguration configuration)
            where TConfig : class, new()
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (configuration is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            var instance = new TConfig();
            configuration.Bind(instance);
            config.Bind<TConfig>().ToConstant(instance).InSingletonScope();
            return config;
        }
    }
}
