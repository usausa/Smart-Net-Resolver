namespace Smart.Resolver;

using Smart.Reflection;
using Smart.Resolver.Builders;
using Smart.Resolver.Handlers;
using Smart.Resolver.Injectors;
using Smart.Resolver.Processors;

public static class ResolverConfigExtensions
{
    public static SmartResolver ToResolver(this IResolverConfig config)
    {
        return new(config);
    }

    public static ResolverConfig UseDelegateFactory<T>(this ResolverConfig config)
        where T : IDelegateFactory
    {
        config.Components.Add<IDelegateFactory, T>();
        return config;
    }

    public static ResolverConfig UseDelegateFactory(this ResolverConfig config, IDelegateFactory factory)
    {
        config.Components.Add(typeof(IDelegateFactory), factory);
        return config;
    }

    public static ResolverConfig UseFactoryBuilder<T>(this ResolverConfig config)
        where T : IFactoryBuilder
    {
        config.Components.Add<IFactoryBuilder, T>();
        return config;
    }

    public static ResolverConfig UseFactoryBuilder(this ResolverConfig config, IFactoryBuilder builder)
    {
        config.Components.Add(typeof(IFactoryBuilder), builder);
        return config;
    }

    public static ResolverConfig UseMetadataFactory(this ResolverConfig config, IDelegateFactory delegateFactory)
    {
        config.Components.Add(typeof(IDelegateFactory), delegateFactory);
        return config;
    }

    public static ResolverConfig UseProcessor<T>(this ResolverConfig config)
        where T : IProcessor
    {
        config.Components.Add<IProcessor, T>();
        return config;
    }

    public static ResolverConfig UseProcessor(this ResolverConfig config, IProcessor processor)
    {
        config.Components.Add(typeof(IProcessor), processor);
        return config;
    }

    public static ResolverConfig UseInjector<T>(this ResolverConfig config)
        where T : IInjector
    {
        config.Components.Add<IInjector, T>();
        return config;
    }

    public static ResolverConfig UseInjector(this ResolverConfig config, IInjector injector)
    {
        config.Components.Add(typeof(IInjector), injector);
        return config;
    }

    public static ResolverConfig UseAutoBinding(this ResolverConfig config)
    {
        config.Components.Add<IMissingHandler, SelfMissingHandler>();
        return config;
    }

    public static ResolverConfig UseAutoBinding(this ResolverConfig config, IEnumerable<Type> ignoreTypes)
    {
        config.Components.Add<IMissingHandler>(new SelfMissingHandler(ignoreTypes));
        return config;
    }

    public static ResolverConfig UseOpenGenericBinding(this ResolverConfig config)
    {
        config.Components.Add<IMissingHandler, OpenGenericMissingHandler>();
        return config;
    }

    public static ResolverConfig UseOpenGenericBinding(this ResolverConfig config, IEnumerable<Type> ignoreTypes)
    {
        config.Components.Add<IMissingHandler>(new OpenGenericMissingHandler(ignoreTypes));
        return config;
    }

    public static ResolverConfig UseArrayBinding(this ResolverConfig config)
    {
        config.Components.Add<IMissingHandler, ArrayMissingHandler>();
        return config;
    }

    public static ResolverConfig UseArrayBinding(this ResolverConfig config, IEnumerable<Type> ignoreElementTypes)
    {
        config.Components.Add<IMissingHandler>(new ArrayMissingHandler(ignoreElementTypes));
        return config;
    }

    public static ResolverConfig UseAssignableBinding(this ResolverConfig config)
    {
        config.Components.Add<IMissingHandler, AssignableMissingHandler>();
        return config;
    }

    public static ResolverConfig UseAssignableBinding(this ResolverConfig config, IEnumerable<Type> targetTypes)
    {
        config.Components.Add<IMissingHandler>(new AssignableMissingHandler(targetTypes));
        return config;
    }

    public static ResolverConfig UseMissingHandler<T>(this ResolverConfig config)
        where T : IMissingHandler
    {
        config.Components.Add<IMissingHandler, T>();
        return config;
    }

    public static ResolverConfig UseMissingHandler(this ResolverConfig config, IMissingHandler handler)
    {
        config.Components.Add(typeof(IMissingHandler), handler);
        return config;
    }

    public static ResolverConfig UsePropertyInjector(this ResolverConfig config)
    {
        config.Components.Add<IInjector, PropertyInjector>();
        return config;
    }

    public static ResolverConfig UseInitializeProcessor(this ResolverConfig config)
    {
        config.Components.Add<IProcessor, InitializeProcessor>();
        return config;
    }

    public static ResolverConfig UseInitializeProcessor(this ResolverConfig config, int order)
    {
        config.Components.Add<IProcessor>(new InitializeProcessor(order));
        return config;
    }
}
