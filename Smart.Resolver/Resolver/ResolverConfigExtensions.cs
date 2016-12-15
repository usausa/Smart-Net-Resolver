namespace Smart.Resolver
{
    using System;

    using Smart.Resolver.Activators;
    using Smart.Resolver.Handlers;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;

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

        public static ResolverConfig UseActivator<T>(this ResolverConfig config)
            where T : IActivator
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IActivator), typeof(T));
            return config;
        }

        public static ResolverConfig UseActivator(this ResolverConfig config, IActivator activator)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IActivator), activator);
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

        public static ResolverConfig UseInitializeActivator(this ResolverConfig config)
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
