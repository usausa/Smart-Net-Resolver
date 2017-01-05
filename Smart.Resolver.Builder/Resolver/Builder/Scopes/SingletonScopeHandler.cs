namespace Smart.Resolver.Builder.Scopes
{
    using Smart.Resolver.Configs;

    /// <summary>
    ///
    /// </summary>
    public class SingletonScopeHandler : IScopeHandler
    {
        /// <summary>
        ///
        /// </summary>
        public string Name => "singleton";

        /// <summary>
        ///
        /// </summary>
        /// <param name="syntax"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public void SetScope(IBindingInSyntax syntax)
        {
            syntax.InSingletonScope();
        }
    }
}
