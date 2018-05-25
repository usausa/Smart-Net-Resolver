namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Smart.Reflection;
    using Smart.Resolver.Handlers;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Metadatas;
    using Smart.Resolver.Processors;

    /// <summary>
    ///
    /// </summary>
    public static class ResolverConfigExtensions
    {
        public static SmartResolver ToResolver(this IResolverConfig config)
        {
            return new SmartResolver(config);
        }

        public static ResolverConfig UseDelegateFactory<T>(this ResolverConfig config)
            where T : IDelegateFactory
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IDelegateFactory), typeof(T));
            return config;
        }

        public static ResolverConfig UseMetadataFactory(this ResolverConfig config, IDelegateFactory delegateFactory)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IDelegateFactory), delegateFactory);
            return config;
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
            where T : IOldProcessor
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IOldProcessor), typeof(T));
            return config;
        }

        public static ResolverConfig UseProcessor(this ResolverConfig config, IOldProcessor activator)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IOldProcessor), activator);
            return config;
        }

        public static ResolverConfig UseInjector<T>(this ResolverConfig config)
            where T : IOldInjector
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IOldInjector), typeof(T));
            return config;
        }

        public static ResolverConfig UseInjector(this ResolverConfig config, IOldInjector injector)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IOldInjector), injector);
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

        public static ResolverConfig UseAutoBinding(this ResolverConfig config, IEnumerable<Type> ignoreTypes)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler>(new SelfMissingHandler(ignoreTypes));
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

        public static ResolverConfig UseOpenGenericBinding(this ResolverConfig config, IEnumerable<Type> ignoreTypes)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler>(new OpenGenericMissingHandler(ignoreTypes));
            return config;
        }

        public static ResolverConfig UseArrayBinding(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler, ArrayMissingHandler>();
            return config;
        }

        public static ResolverConfig UseArrayBinding(this ResolverConfig config, IEnumerable<Type> ignoreElementTypes)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler>(new ArrayMissingHandler(ignoreElementTypes));
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

        public static ResolverConfig UseAssignableBinding(this ResolverConfig config, IEnumerable<Type> ignoreTypes)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler>(new AssignableMissingHandler(ignoreTypes));
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

            config.Components.Add<IOldInjector, OldPropertyInjector>();
            return config;
        }

        public static ResolverConfig UseInitializeProcessor(this ResolverConfig config)
        {
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IOldInjector, OldPropertyInjector>();
            return config;
        }
    }
}
