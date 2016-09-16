namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public interface IMissingPipeline
    {
        /// <summary>
        ///
        /// </summary>
        IList<IBindingResolver> Resolvers { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        IEnumerable<IBinding> Resolve(Type type);
    }
}
