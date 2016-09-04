namespace Smart.Resolver.Scopes
{
    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public interface IScope
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        IScopeStorage GetStorage(IKernel kernel);
    }
}
