namespace Smart.Resolver.Constraints;

using Smart.Resolver.Bindings;

public interface IConstraint
{
    bool IsMultiKey => false;

    bool Match(BindingMetadata metadata, object? key);
}
