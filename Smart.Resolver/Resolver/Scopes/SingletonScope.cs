namespace Smart.Resolver.Scopes
{
    /// <summary>
    ///
    /// </summary>
    public class SingletonScope : IScope
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public IScopeStorage GetStorage(IKernel kernel)
        {
            return kernel.Components.Get<ISingletonScopeStorage>();
        }
    }
}
