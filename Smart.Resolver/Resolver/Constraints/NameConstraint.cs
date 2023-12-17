namespace Smart.Resolver.Constraints;

using Smart.Resolver.Bindings;

public sealed class NameConstraint : IConstraint
{
    private readonly string name;

    public NameConstraint(string name)
    {
        this.name = name;
    }

    public bool Match(BindingMetadata metadata) => name == metadata.Name;

    public override bool Equals(object? obj)
    {
        return obj is NameConstraint constraint && name == constraint.name;
    }

    public override int GetHashCode() => name.GetHashCode(StringComparison.Ordinal);
}
