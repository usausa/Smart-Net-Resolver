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

        public static ResolverConfig UseBindingContextInjectProcessor(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor, BindingContextInjectProcessor>();
            return config;
        }

        public static ResolverConfig UseBindingContextInjectProcessor(this ResolverConfig config, int order)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor>(new BindingContextInjectProcessor(order));
            return config;
        }

        public static ResolverConfig UseBindingContextInitializeProcessor(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor, BindingContextInitializeProcessor>();
            return config;
        }

        public static ResolverConfig UseBindingContextInitializeProcessor(this ResolverConfig config, int order)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor>(new BindingContextInitializeProcessor(order));
            return config;
        }
    }
}
