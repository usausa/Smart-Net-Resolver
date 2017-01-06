namespace Smart.Resolver.Builder.Scopes
{
    using Smart.Resolver.Configs;

    /// <summary>
    ///
    /// </summary>
    public class SingletonScopeProcessor : IScopeProcessor
    {
        /// <summary>
        ///
        /// </summary>
        public string Name => "singleton";

        /// <summary>
        ///
        /// </summary>
        /// <param name="syntax"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public IBindingNamedWithSyntax Process(IBindingInSyntax syntax)
        {
            return syntax.InSingletonScope();
        }
    }
}
