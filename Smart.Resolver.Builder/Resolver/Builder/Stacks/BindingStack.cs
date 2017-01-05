namespace Smart.Resolver.Builder.Stacks
{
    using Smart.Resolver.Configs;

    /// <summary>
    ///
    /// </summary>
    public class BindingStack
    {
        /// <summary>
        ///
        /// </summary>
        public IBindingToSyntax<object> Syntax { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="syntax"></param>
        public BindingStack(IBindingToSyntax<object> syntax)
        {
            Syntax = syntax;
        }
    }
}
