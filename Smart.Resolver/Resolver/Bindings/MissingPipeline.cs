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
        /// <param name="context"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IEnumerable<IBinding> Resolve(IResolverContext context, Type type)
        {
            return Resolvers.SelectMany(x => x.Resolve(context, type));
        }
    }
}
