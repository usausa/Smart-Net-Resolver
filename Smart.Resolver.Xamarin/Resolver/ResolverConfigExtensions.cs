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

        public static ResolverConfig UseBindingContextProcessor(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor, BindingContextProcessor>();
            return config;
        }

        public static ResolverConfig UseBindingContextProcessor(this ResolverConfig config, int order)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor>(new BindingContextProcessor(order));
            return config;
        }
    }
}
