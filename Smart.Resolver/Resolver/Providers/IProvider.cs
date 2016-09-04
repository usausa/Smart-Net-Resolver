namespace Smart.Resolver.Providers
{
    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public interface IProvider
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        object Create(IKernel kernel, IBinding binding);
    }
}
