namespace Smart.Resolver.Configuration;

using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

[RequiresUnreferencedCode("Resolver definitions resolve types by name at runtime, so their constructors and properties may be trimmed.")]
[RequiresDynamicCode("Resolver definitions in JSON are deserialized with reflection.")]
internal static class JsonDefinitionLoader
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter(JsonNamingPolicy.CamelCase, allowIntegerValues: false) }
    };

    internal static ResolverDefinition Parse(string json) =>
        JsonSerializer.Deserialize<ResolverDefinition>(json, JsonOptions) ?? new ResolverDefinition();

    internal static ResolverDefinition ParseStream(Stream stream)
    {
        using var reader = new StreamReader(stream, leaveOpen: true);
        return Parse(reader.ReadToEnd());
    }
}
