namespace Smart.Resolver;

using Smart.Resolver.Configs;
using Smart.Resolver.Constraints;

public static class ConstraintExtensions
{
    public static IBindingWithSyntax Named(
        this IBindingConstraintSyntax syntax,
        object key)
    {
        return syntax.Constraint(new KeyConstraint(key));
    }
}
