namespace Smart.Resolver.Builder.Scopes
{
    using Smart.Resolver.Configs;

    /// <summary>
    ///
    /// </summary>
    public class TransientScopeHandler : IScopeHandler
    {
        /// <summary>
        ///
        /// </summary>
        public string Name => "singleton";

        /// <summary>
        ///
        /// </summary>
        /// <param name="syntax"></param>
        public void SetScope(IBindingInSyntax syntax)
        {
            syntax.InTransientScope();
        }
    }
}
