namespace Smart.Resolver
{
    using Smart.Resolver.Configs;

    public static class BindingExtensions
    {
        public static IBindingNamedWithSyntax InRequestScope(this IBindingInSyntax syntax)
        {
            return syntax.InScope(c => new RequestScope());
        }
    }
}
