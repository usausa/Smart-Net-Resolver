namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public interface IResolverContext
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<IBinding> FindBindings(Type type);
    }
}
