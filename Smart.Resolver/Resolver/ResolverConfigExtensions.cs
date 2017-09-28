namespace Smart.Resolver
{
    using System;

    using Smart.Resolver.Handlers;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;
    using Smart.Resolver.Processors;

    /// <summary>
    ///
    /// </summary>
    public static class ResolverConfigExtensions
    {
        public static StandardResolver ToResolver(this IResolverConfig config)
        {
            return new StandardResolver(config);
        }

        public static ResolverConfig UseMetadataFactory<T>(this ResolverConfig config)
            where T : IMetadataFactory
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IMetadataFactory), typeof(T));
            return config;
        }

        public static ResolverConfig UseMetadataFactory(this ResolverConfig config, IMetadataFactory metadataFactory)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IMetadataFactory), metadataFactory);
            return config;
        }

        public static ResolverConfig UseProcessor<T>(this ResolverConfig config)
            where T : IProcessor
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IProcessor), typeof(T));
            return config;
        }

        public static ResolverConfig UseProcessor(this ResolverConfig config, IProcessor activator)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IProcessor), activator);
            return config;
        }

        public static ResolverConfig UseInjector<T>(this ResolverConfig config)
            where T : IInjector
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IInjector), typeof(T));
            return config;
        }

        public static ResolverConfig UseInjector(this ResolverConfig config, IInjector injector)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IInjector), injector);
            return config;
        }

        public static ResolverConfig UseAutoBinding(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler, SelfMissingHandler>();
            return config;
        }

        public static ResolverConfig UseOpenGenericBinding(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler, OpenGenericMissingHandler>();
            return config;
        }

        public static ResolverConfig UseAssignableBinding(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler, AssignableMissingHandler>();
            return config;
        }

        public static ResolverConfig UseMissingHandler<T>(this ResolverConfig config)
            where T : IMissingHandler
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IMissingHandler), typeof(T));
            return config;
        }

        public static ResolverConfig UseMissingHandler(this ResolverConfig config, IMissingHandler handler)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IMissingHandler), handler);
            return config;
        }

        public static ResolverConfig UsePropertyInjector(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IInjector, PropertyInjector>();
            return config;
        }

        public static ResolverConfig UseInitializeProcessor(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IInjector, PropertyInjector>();
            return config;
        }
    }
}
