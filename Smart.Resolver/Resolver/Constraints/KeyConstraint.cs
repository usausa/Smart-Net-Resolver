namespace Smart.Resolver.Constraints;

using Smart.Resolver.Bindings;

public sealed class KeyConstraint : IConstraint
{
    private readonly object? serviceKey;

    public KeyConstraint(object? serviceKey)
    {
        this.serviceKey = serviceKey;
    }

    public bool Match(BindingMetadata metadata, object? key) => key?.Equals(serviceKey) ?? false;
}
