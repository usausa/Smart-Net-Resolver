namespace Smart.Resolver.Mocks;

using Smart.Resolver.Attributes;

public sealed class HasPropertyObject
{
    [Inject]
    public SimpleObject? Injected { get; set; }
}
