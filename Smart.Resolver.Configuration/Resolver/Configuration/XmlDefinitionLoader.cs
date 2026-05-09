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
            ServiceType = element.Attribute("serviceType")?.Value ?? string.Empty,
            TargetKind = ParseEnum(element.Attribute("targetKind")?.Value, BindingTargetKind.Type),
            ImplementationType = element.Attribute("implementationType")?.Value,
            ConstantValue = element.Attribute("constantValue")?.Value,
            ConstantValueType = element.Attribute("constantValueType")?.Value,
            Scope = ParseEnum(element.Attribute("scope")?.Value, ScopeKind.Transient),
            ConstraintKey = element.Attribute("constraintKey")?.Value,
            Metadata = element.Elements("metadata").Select(ParseMetadata).ToList(),
            ConstructorArguments = element.Elements("constructorArgument").Select(ParseParameter).ToList(),
            PropertyValues = element.Elements("propertyValue").Select(ParseParameter).ToList()
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
