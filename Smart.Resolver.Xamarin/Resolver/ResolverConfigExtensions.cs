namespace Smart.Resolver
{
    using System;

    using Smart.Resolver.Handlers;
    using Smart.Resolver.Processors;

    /// <summary>
    ///
    /// </summary>
    public static class ResolverConfigExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static ResolverConfig UseDependencyService(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            return config.UseMissingHandler<DependencyServiceMissingHandler>();
        }

        public static ResolverConfig UseBindingContextInject(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor, BindingContextInjectProcessor>();
            return config;
        }
    }
}
