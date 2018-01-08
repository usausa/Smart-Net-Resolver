namespace Smart.Resolver.Scopes
{
    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Factories;

    /// <summary>
    ///
    /// </summary>
    public interface IScope
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        IScope Copy(IComponentContainer components);

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        IObjectFactory Create(IKernel kernel, IBinding binding, IObjectFactory factory);
    }
}
