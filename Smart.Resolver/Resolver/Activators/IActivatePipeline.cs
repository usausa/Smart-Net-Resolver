namespace Smart.Resolver.Activators
{
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public interface IActivatePipeline
    {
        /// <summary>
        ///
        /// </summary>
        IList<IActivator> Activators { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        void Activate(object instance);
    }
}
