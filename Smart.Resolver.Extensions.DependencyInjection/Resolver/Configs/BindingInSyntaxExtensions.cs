namespace Smart.Resolver.Configs
{
    using Smart.Resolver.Scope;

    public static class BindingInSyntaxExtensions
    {
        public static IBindingNamedWithSyntax InContextScope(this IBindingInSyntax syntax)
        {
            return syntax.InScope(c => new ContextScope());
        }
    }
}
