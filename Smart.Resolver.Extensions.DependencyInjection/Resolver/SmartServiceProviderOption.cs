namespace Smart.Resolver;

public sealed class SmartServiceProviderOption
{
    public bool DisposalTracking { get; init; } = true;

    public bool RootScope { get; init; } = true;
}
