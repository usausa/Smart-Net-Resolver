namespace Smart.Resolver.Mocks;

using Smart.Resolver.Attributes;

public sealed class ServiceWithProperty
{
    [Inject]
    public string? Name { get; set; }

    [Inject]
    public int Count { get; set; }
}
