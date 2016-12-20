namespace Smart.Resolver
{
    using Smart.ComponentModel;

    /// <summary>
    ///
    /// </summary>
    public interface IKernel : IResolver
    {
        /// <summary>
        ///
        /// </summary>
        IComponentContainer Components { get; }
    }
}
