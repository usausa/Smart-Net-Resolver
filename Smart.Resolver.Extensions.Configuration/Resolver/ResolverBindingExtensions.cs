namespace Smart.Resolver
{
    using Microsoft.Extensions.Configuration;

    public static class ResolverBindingExtensions
    {
        public static ResolverConfig BindConfig<TConfig>(
            this ResolverConfig config,
            IConfiguration configuration)
            where TConfig : class, new()
        {
            var instance = new TConfig();
            configuration.Bind(instance);
            config.Bind<TConfig>().ToConstant(instance).InSingletonScope();
            return config;
        }
    }
}
