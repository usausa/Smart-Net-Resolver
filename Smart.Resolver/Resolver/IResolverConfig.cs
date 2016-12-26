namespace Smart.Resolver
{
    using System.Collections.Generic;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public interface IResolverConfig
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IComponentContainer CreateComponentContainer();

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        IEnumerable<IBinding> CreateBindings(IComponentContainer components);
    }
}
