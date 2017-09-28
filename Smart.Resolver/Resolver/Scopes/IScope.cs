namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.ComponentModel;
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
        /// <param name="binding"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        object GetOrAdd(IKernel kernel, IBinding binding, Func<IBinding, object> factory);

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        IScope Copy(IComponentContainer components);
    }
}
