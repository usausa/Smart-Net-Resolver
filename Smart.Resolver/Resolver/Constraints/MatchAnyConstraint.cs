namespace Smart.Resolver.Constraints;

using Smart.Resolver.Bindings;

public sealed class MatchAnyConstraint : IConstraint
{
    public static readonly MatchAnyConstraint Instance = new();

    private MatchAnyConstraint()
    {
    }

    public bool IsMultiKey => true;

    public bool Match(BindingMetadata metadata, object? key) => true;
}
