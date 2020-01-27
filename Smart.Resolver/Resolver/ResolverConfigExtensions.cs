namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;

    using Smart.Reflection;
    using Smart.Resolver.Builders;
    using Smart.Resolver.Handlers;
    using Smart.Resolver.Injectors;
    using Smart.Resolver.Processors;

    public static class ResolverConfigExtensions
    {
        public static SmartResolver ToResolver(this IResolverConfig config)
        {
            return new SmartResolver(config);
        }

        public static ResolverConfig UseDelegateFactory<T>(this ResolverConfig config)
            where T : IDelegateFactory
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IDelegateFactory), typeof(T));
            return config;
        }

        public static ResolverConfig UseDelegateFactory(this ResolverConfig config, IDelegateFactory factory)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IDelegateFactory), factory);
            return config;
        }

        public static ResolverConfig UseFactoryBuilder<T>(this ResolverConfig config)
            where T : IFactoryBuilder
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IFactoryBuilder), typeof(T));
            return config;
        }

        public static ResolverConfig UseFactoryBuilder(this ResolverConfig config, IFactoryBuilder builder)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IFactoryBuilder), builder);
            return config;
        }

        public static ResolverConfig UseMetadataFactory(this ResolverConfig config, IDelegateFactory delegateFactory)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IDelegateFactory), delegateFactory);
            return config;
        }

        public static ResolverConfig UseProcessor<T>(this ResolverConfig config)
            where T : IProcessor
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IProcessor), typeof(T));
            return config;
        }

        public static ResolverConfig UseProcessor(this ResolverConfig config, IProcessor processor)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IProcessor), processor);
            return config;
        }

        public static ResolverConfig UseInjector<T>(this ResolverConfig config)
            where T : IInjector
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IInjector), typeof(T));
            return config;
        }

        public static ResolverConfig UseInjector(this ResolverConfig config, IInjector injector)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IInjector), injector);
            return config;
        }

        public static ResolverConfig UseAutoBinding(this ResolverConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler, SelfMissingHandler>();
            return config;
        }

        public static ResolverConfig UseAutoBinding(this ResolverConfig config, IEnumerable<Type> ignoreTypes)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler>(new SelfMissingHandler(ignoreTypes));
            return config;
        }

        public static ResolverConfig UseOpenGenericBinding(this ResolverConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler, OpenGenericMissingHandler>();
            return config;
        }

        public static ResolverConfig UseOpenGenericBinding(this ResolverConfig config, IEnumerable<Type> ignoreTypes)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler>(new OpenGenericMissingHandler(ignoreTypes));
            return config;
        }

        public static ResolverConfig UseArrayBinding(this ResolverConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler, ArrayMissingHandler>();
            return config;
        }

        public static ResolverConfig UseArrayBinding(this ResolverConfig config, IEnumerable<Type> ignoreElementTypes)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler>(new ArrayMissingHandler(ignoreElementTypes));
            return config;
        }

        public static ResolverConfig UseAssignableBinding(this ResolverConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler, AssignableMissingHandler>();
            return config;
        }

        public static ResolverConfig UseAssignableBinding(this ResolverConfig config, IEnumerable<Type> targetTypes)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IMissingHandler>(new AssignableMissingHandler(targetTypes));
            return config;
        }

        public static ResolverConfig UseMissingHandler<T>(this ResolverConfig config)
            where T : IMissingHandler
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IMissingHandler), typeof(T));
            return config;
        }

        public static ResolverConfig UseMissingHandler(this ResolverConfig config, IMissingHandler handler)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add(typeof(IMissingHandler), handler);
            return config;
        }

        public static ResolverConfig UsePropertyInjector(this ResolverConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IInjector, PropertyInjector>();
            return config;
        }

        public static ResolverConfig UseInitializeProcessor(this ResolverConfig config)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor, InitializeProcessor>();
            return config;
        }

        public static ResolverConfig UseInitializeProcessor(this ResolverConfig config, int order)
        {
            if (config is null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            config.Components.Add<IProcessor>(new InitializeProcessor(order));
            return config;
        }
    }
}
