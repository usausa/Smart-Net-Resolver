namespace Smart.Resolver
{
    using System;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public static class StandardResolverExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public static StandardResolver UseDependencyService(this StandardResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException(nameof(resolver));
            }

            return resolver.Configure(c => c.Get<IMissingPipeline>().Resolvers.Add(new DependencyServiceBindingResolver()));
        }
    }
}
