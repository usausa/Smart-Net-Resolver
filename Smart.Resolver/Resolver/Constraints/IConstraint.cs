namespace Smart.Resolver.Constraints;

using Smart.Resolver.Bindings;

public interface IConstraint
{
    bool Match(BindingMetadata metadata, object? key);
}
