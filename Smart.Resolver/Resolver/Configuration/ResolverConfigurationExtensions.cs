namespace Smart.Resolver.Configuration;

using System.Diagnostics.CodeAnalysis;

public static class ResolverConfigurationExtensions
{
    //--------------------------------------------------------------------------------
    // JSON
    //--------------------------------------------------------------------------------

    [RequiresUnreferencedCode("Resolver definitions resolve types by name at runtime, so their constructors and properties may be trimmed.")]
    [RequiresDynamicCode("Resolver definitions in JSON are deserialized with reflection.")]
    public static ResolverConfig LoadJson(
        this ResolverConfig config,
        string json,
        Func<string, Type>? typeResolver = null)
    {
        DefinitionApplier.Apply(config, JsonDefinitionLoader.Parse(json), typeResolver);
        return config;
    }

    [RequiresUnreferencedCode("Resolver definitions resolve types by name at runtime, so their constructors and properties may be trimmed.")]
    [RequiresDynamicCode("Resolver definitions in JSON are deserialized with reflection.")]
    public static ResolverConfig LoadJson(
        this ResolverConfig config,
        Stream stream,
        Func<string, Type>? typeResolver = null)
    {
        DefinitionApplier.Apply(config, JsonDefinitionLoader.ParseStream(stream), typeResolver);
        return config;
    }

    [RequiresUnreferencedCode("Resolver definitions resolve types by name at runtime, so their constructors and properties may be trimmed.")]
    [RequiresDynamicCode("Resolver definitions in JSON are deserialized with reflection.")]
    public static ResolverConfig LoadJsonFile(
        this ResolverConfig config,
        string path,
        Func<string, Type>? typeResolver = null)
    {
        using var stream = File.OpenRead(path);
        return config.LoadJson(stream, typeResolver);
    }

    //--------------------------------------------------------------------------------
    // XML
    //--------------------------------------------------------------------------------

    [RequiresUnreferencedCode("Resolver definitions resolve types by name at runtime, so their constructors and properties may be trimmed.")]
    public static ResolverConfig LoadXml(
        this ResolverConfig config,
        string xml,
        Func<string, Type>? typeResolver = null)
    {
        DefinitionApplier.Apply(config, XmlDefinitionLoader.Parse(xml), typeResolver);
        return config;
    }

    [RequiresUnreferencedCode("Resolver definitions resolve types by name at runtime, so their constructors and properties may be trimmed.")]
    public static ResolverConfig LoadXml(
        this ResolverConfig config,
        Stream stream,
        Func<string, Type>? typeResolver = null)
    {
        DefinitionApplier.Apply(config, XmlDefinitionLoader.ParseStream(stream), typeResolver);
        return config;
    }

    [RequiresUnreferencedCode("Resolver definitions resolve types by name at runtime, so their constructors and properties may be trimmed.")]
    public static ResolverConfig LoadXmlFile(
        this ResolverConfig config,
        string path,
        Func<string, Type>? typeResolver = null)
    {
        using var stream = File.OpenRead(path);
        return config.LoadXml(stream, typeResolver);
    }

    //--------------------------------------------------------------------------------
    // Direct
    //--------------------------------------------------------------------------------

    [RequiresUnreferencedCode("Resolver definitions resolve types by name at runtime, so their constructors and properties may be trimmed.")]
    public static ResolverConfig LoadDefinition(
        this ResolverConfig config,
        ResolverDefinition definition,
        Func<string, Type>? typeResolver = null)
    {
        DefinitionApplier.Apply(config, definition, typeResolver);
        return config;
    }
}
