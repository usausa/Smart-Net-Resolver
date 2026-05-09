namespace Smart.Resolver;

using Smart.Resolver.Constraints;
using Smart.Resolver.Expressions;

public static class ConstraintExtensions
{
    public static IBindingWithSyntax Keyed(
        this IBindingConstraintSyntax syntax,
        object? key)
    {
        return syntax.Constraint(new KeyConstraint(key));
    }
}
