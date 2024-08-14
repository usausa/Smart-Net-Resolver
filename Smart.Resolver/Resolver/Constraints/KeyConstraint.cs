namespace Smart.Resolver.Constraints;

using Smart.Resolver.Bindings;

public sealed class KeyConstraint : IConstraint
{
    private readonly object key;

    public KeyConstraint(object key)
    {
        this.key = key;
    }

    public bool Match(BindingMetadata metadata, object? parameter) => parameter == key;
}
