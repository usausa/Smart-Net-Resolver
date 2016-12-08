namespace Smart.Resolver
{
    using Smart.Resolver.Bindings;

    public static class BindingExtensions
    {
        public static IBindingNamedWithSyntax InRequestScope(this IBindingInSyntax syntax)
        {
            return syntax.InScope(new RequestScope());
        }
    }
}
