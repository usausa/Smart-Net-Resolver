namespace Smart.Resolver
{
    using System;

    using Smart.Resolver.Handlers;
    using Smart.Resolver.Processors;

    public static class ResolverConfigExtensions
    {
        public static ResolverConfig UseDependencyService(this ResolverConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            return config.UseMissingHandler<DependencyServiceMissingHandler>();
        }

        public static ResolverConfig UseBindingContextInjectProcessor(this ResolverConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor, BindingContextInjectProcessor>();
            return config;
        }

        public static ResolverConfig UseBindingContextInjectProcessor(this ResolverConfig config, int order)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor>(new BindingContextInjectProcessor(order));
            return config;
        }

        public static ResolverConfig UseBindingContextInitializeProcessor(this ResolverConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor, BindingContextInitializeProcessor>();
            return config;
        }

        public static ResolverConfig UseBindingContextInitializeProcessor(this ResolverConfig config, int order)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor>(new BindingContextInitializeProcessor(order));
            return config;
        }
    }
}
