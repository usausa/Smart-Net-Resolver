namespace Smart.Resolver.Configuration;

using System.Text.Json;
using System.Text.Json.Serialization;

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
