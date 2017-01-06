namespace Smart.Resolver.Builder.Scopes
{
    using Smart.Resolver.Configs;

    /// <summary>
    ///
    /// </summary>
    public interface IScopeProcessor
    {
        /// <summary>
        ///
        /// </summary>
        string Name { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="syntax"></param>
        /// <returns></returns>
        IBindingNamedWithSyntax Process(IBindingInSyntax syntax);
    }
}
