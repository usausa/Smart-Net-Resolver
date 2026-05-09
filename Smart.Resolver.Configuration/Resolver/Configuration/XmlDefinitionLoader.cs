namespace Smart.Resolver.Configuration;

using System.Xml.Linq;

internal static class XmlDefinitionLoader
{
    internal static ResolverDefinition Parse(string xml)
    {
        var doc = XDocument.Parse(xml);
        return ParseDocument(doc);
    }

    internal static ResolverDefinition ParseStream(Stream stream)
    {
        var doc = XDocument.Load(stream);
        return ParseDocument(doc);
    }

    private static ResolverDefinition ParseDocument(XDocument document)
    {
        var root = document.Root;
        if (root is null)
        {
            return new ResolverDefinition();
        }

        var bindings = root.Elements("binding").Select(ParseBinding).ToList();
        return new ResolverDefinition { Bindings = bindings };
    }

    private static BindingDefinition ParseBinding(XElement element)
    {
        return new BindingDefinition
        {
            Key = element.Attribute("key")?.Value,
            Scope = ParseEnum(element.Attribute("scope")?.Value, ScopeKind.Transient),
            BindingTarget = ParseEnum(element.Attribute("bindingTarget")?.Value, BindingTargetKind.Type),
            Service = element.Attribute("service")?.Value ?? string.Empty,
            Implementation = element.Attribute("implementation")?.Value,
            Constant = element.Attribute("constant")?.Value,
            ConstantType = element.Attribute("constantType")?.Value,
            ConstructorArguments = element.Elements("constructorArgument").Select(ParseParameter).ToList(),
            PropertyValues = element.Elements("propertyValue").Select(ParseParameter).ToList(),
            Metadata = element.Elements("metadata").Select(ParseMetadata).ToList()
        };
    }

    private static MetadataEntry ParseMetadata(XElement element) => new()
    {
        Key = element.Attribute("key")?.Value ?? string.Empty,
        Value = element.Attribute("value")?.Value,
        ValueType = element.Attribute("valueType")?.Value
    };

    private static ParameterEntry ParseParameter(XElement element) => new()
    {
        Name = element.Attribute("name")?.Value ?? string.Empty,
        Kind = ParseEnum(element.Attribute("kind")?.Value, ParameterKind.Constant),
        Value = element.Attribute("value")?.Value,
        ValueType = element.Attribute("valueType")?.Value
    };

    private static TEnum ParseEnum<TEnum>(string? value, TEnum defaultValue)
        where TEnum : struct, Enum =>
        value is not null && Enum.TryParse<TEnum>(value, ignoreCase: true, out var result) ? result : defaultValue;
}
