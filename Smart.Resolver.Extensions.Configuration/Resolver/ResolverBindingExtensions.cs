namespace Smart.Resolver;

using System.Diagnostics.CodeAnalysis;

using Microsoft.Extensions.Configuration;

public static class ResolverBindingExtensions
{
    [RequiresUnreferencedCode("BindConfig binds TConfig with the reflection-based configuration binder, so members of TConfig and its dependent types may be trimmed.")]
    [RequiresDynamicCode("Binding TConfig to configuration values may require generating dynamic code at runtime.")]
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
