namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    ///
    /// </summary>
    public class MissingPipeline : IMissingPipeline
    {
        /// <summary>
        ///
        /// </summary>
        public IList<IBindingResolver> Resolvers { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resolvers"></param>
        public MissingPipeline(params IBindingResolver[] resolvers)
        {
            Resolvers = resolvers.ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IBinding> Resolve(Type type)
        {
            return Resolvers.Select(_ => _.Resolve(type)).Where(_ => _ != null);
        }
    }
}
