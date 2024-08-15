namespace Smart.Resolver;

using Smart.Resolver.Configs;

public static class ResolverBindExtensions
{
    // ------------------------------------------------------------
    // Transient
    // ------------------------------------------------------------

    public static ResolverConfig BindTransient(
        this ResolverConfig config,
        Type serviceType,
        Type implementationType)
    {
        config.Bind(serviceType).To(implementationType).InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient(
        this ResolverConfig config,
        Type serviceType,
        Type implementationType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).To(implementationType);
        option(syntax);
        syntax.InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient(
        this ResolverConfig config,
        Type serviceType)
    {
        config.Bind(serviceType).ToSelf().InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient(
        this ResolverConfig config,
        Type serviceType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToSelf();
        option(syntax);
        syntax.InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService, TImplementation>(this ResolverConfig config)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().To<TImplementation>().InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService, TImplementation>(
        this ResolverConfig config,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().To<TImplementation>();
        option(syntax);
        syntax.InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService>(this ResolverConfig config)
        where TService : class
    {
        config.Bind<TService>().ToSelf().InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService>(
        this ResolverConfig config,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToSelf();
        option(syntax);
        syntax.InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient(
        this ResolverConfig config,
        Type serviceType,
        object value)
    {
        config.Bind(serviceType).ToConstant(value).InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient(
        this ResolverConfig config,
        Type serviceType,
        object value,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToConstant(value);
        option(syntax);
        syntax.InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService>(
        this ResolverConfig config,
        TService value)
        where TService : class
    {
        config.Bind<TService>().ToConstant(value).InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService>(
        this ResolverConfig config,
        TService value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService, TImplementation>(
        this ResolverConfig config,
        TImplementation value)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToConstant(value).InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService, TImplementation>(
        this ResolverConfig config,
        TImplementation value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient(
        this ResolverConfig config,
        Type serviceType,
        Func<IResolver, object> factory)
    {
        config.Bind(serviceType).ToMethod(factory).InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient(
        this ResolverConfig config,
        Type serviceType,
        Func<IResolver, object> factory,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToMethod(factory);
        option(syntax);
        syntax.InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService>(
        this ResolverConfig config,
        Func<IResolver, TService> factory)
        where TService : class
    {
        config.Bind<TService>().ToMethod(factory).InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService>(
        this ResolverConfig config,
        Func<IResolver, TService> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService, TImplementation>(
        this ResolverConfig config,
        Func<IResolver, TImplementation> factory)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToMethod(factory).InTransientScope();
        return config;
    }

    public static ResolverConfig BindTransient<TService, TImplementation>(
        this ResolverConfig config,
        Func<IResolver, TImplementation> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InTransientScope();
        return config;
    }

    // ------------------------------------------------------------
    // Transient Keyed
    // ------------------------------------------------------------

    public static ResolverConfig BindKeyedTransient(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Type implementationType)
    {
        config.Bind(serviceType).To(implementationType).InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Type implementationType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).To(implementationType);
        option(syntax);
        syntax.InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType)
    {
        config.Bind(serviceType).ToSelf().InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToSelf();
        option(syntax);
        syntax.InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().To<TImplementation>().InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().To<TImplementation>();
        option(syntax);
        syntax.InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient<TService>(
        this ResolverConfig config,
        object? serviceKey)
        where TService : class
    {
        config.Bind<TService>().ToSelf().InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient<TService>(
        this ResolverConfig config,
        object? serviceKey,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToSelf();
        option(syntax);
        syntax.InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        object value)
    {
        config.Bind(serviceType).ToConstant(value).InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        object value,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToConstant(value);
        option(syntax);
        syntax.InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient<TService>(
        this ResolverConfig config,
        object? serviceKey,
        TService value)
        where TService : class
    {
        config.Bind<TService>().ToConstant(value).InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient<TService>(
        this ResolverConfig config,
        object? serviceKey,
        TService value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        TImplementation value)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToConstant(value).InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        TImplementation value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Func<IResolver, object> factory)
    {
        config.Bind(serviceType).ToMethod(factory).InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Func<IResolver, object> factory,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToMethod(factory);
        option(syntax);
        syntax.InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindTBindKeyedTransient<TService>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TService> factory)
        where TService : class
    {
        config.Bind<TService>().ToMethod(factory).InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient<TService>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TService> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TImplementation> factory)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToMethod(factory).InTransientScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedTransient<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TImplementation> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InTransientScope().Keyed(serviceKey);
        return config;
    }

    // ------------------------------------------------------------
    // Singleton
    // ------------------------------------------------------------

    public static ResolverConfig BindSingleton(
        this ResolverConfig config,
        Type serviceType,
        Type implementationType)
    {
        config.Bind(serviceType).To(implementationType).InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton(
        this ResolverConfig config,
        Type serviceType,
        Type implementationType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).To(implementationType);
        option(syntax);
        syntax.InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton(
        this ResolverConfig config,
        Type serviceType)
    {
        config.Bind(serviceType).ToSelf().InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton(
        this ResolverConfig config,
        Type serviceType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToSelf();
        option(syntax);
        syntax.InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService, TImplementation>(this ResolverConfig config)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().To<TImplementation>().InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService, TImplementation>(
        this ResolverConfig config,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().To<TImplementation>();
        option(syntax);
        syntax.InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService>(this ResolverConfig config)
        where TService : class
    {
        config.Bind<TService>().ToSelf().InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService>(
        this ResolverConfig config,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToSelf();
        option(syntax);
        syntax.InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton(
        this ResolverConfig config,
        Type serviceType,
        object value)
    {
        config.Bind(serviceType).ToConstant(value).InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton(
        this ResolverConfig config,
        Type serviceType,
        object value,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToConstant(value);
        option(syntax);
        syntax.InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService>(
        this ResolverConfig config,
        TService value)
        where TService : class
    {
        config.Bind<TService>().ToConstant(value).InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService>(
        this ResolverConfig config,
        TService value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService, TImplementation>(
        this ResolverConfig config,
        TImplementation value)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToConstant(value).InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService, TImplementation>(
        this ResolverConfig config,
        TImplementation value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton(
        this ResolverConfig config,
        Type serviceType,
        Func<IResolver, object> factory)
    {
        config.Bind(serviceType).ToMethod(factory).InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton(
        this ResolverConfig config,
        Type serviceType,
        Func<IResolver, object> factory,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToMethod(factory);
        option(syntax);
        syntax.InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService>(
        this ResolverConfig config,
        Func<IResolver, TService> factory)
        where TService : class
    {
        config.Bind<TService>().ToMethod(factory).InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService>(
        this ResolverConfig config,
        Func<IResolver, TService> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService, TImplementation>(
        this ResolverConfig config,
        Func<IResolver, TImplementation> factory)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToMethod(factory).InSingletonScope();
        return config;
    }

    public static ResolverConfig BindSingleton<TService, TImplementation>(
        this ResolverConfig config,
        Func<IResolver, TImplementation> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InSingletonScope();
        return config;
    }

    // ------------------------------------------------------------
    // Singleton Keyed
    // ------------------------------------------------------------

    public static ResolverConfig BindKeyedSingleton(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Type implementationType)
    {
        config.Bind(serviceType).To(implementationType).InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Type implementationType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).To(implementationType);
        option(syntax);
        syntax.InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType)
    {
        config.Bind(serviceType).ToSelf().InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToSelf();
        option(syntax);
        syntax.InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().To<TImplementation>().InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().To<TImplementation>();
        option(syntax);
        syntax.InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService>(
        this ResolverConfig config,
        object? serviceKey)
        where TService : class
    {
        config.Bind<TService>().ToSelf().InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService>(
        this ResolverConfig config,
        object? serviceKey,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToSelf();
        option(syntax);
        syntax.InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        object value)
    {
        config.Bind(serviceType).ToConstant(value).InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        object value,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToConstant(value);
        option(syntax);
        syntax.InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService>(
        this ResolverConfig config,
        object? serviceKey,
        TService value)
        where TService : class
    {
        config.Bind<TService>().ToConstant(value).InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService>(
        this ResolverConfig config,
        object? serviceKey,
        TService value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        TImplementation value)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToConstant(value).InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        TImplementation value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Func<IResolver, object> factory)
    {
        config.Bind(serviceType).ToMethod(factory).InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Func<IResolver, object> factory,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToMethod(factory);
        option(syntax);
        syntax.InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TService> factory)
        where TService : class
    {
        config.Bind<TService>().ToMethod(factory).InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TService> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TImplementation> factory)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToMethod(factory).InSingletonScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedSingleton<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TImplementation> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InSingletonScope().Keyed(serviceKey);
        return config;
    }

    // ------------------------------------------------------------
    // Container
    // ------------------------------------------------------------

    public static ResolverConfig BindContainer(
        this ResolverConfig config,
        Type serviceType,
        Type implementationType)
    {
        config.Bind(serviceType).To(implementationType).InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer(
        this ResolverConfig config,
        Type serviceType,
        Type implementationType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).To(implementationType);
        option(syntax);
        syntax.InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer(
        this ResolverConfig config,
        Type serviceType)
    {
        config.Bind(serviceType).ToSelf().InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer(
        this ResolverConfig config,
        Type serviceType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToSelf();
        option(syntax);
        syntax.InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService, TImplementation>(this ResolverConfig config)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().To<TImplementation>().InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService, TImplementation>(
        this ResolverConfig config,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().To<TImplementation>();
        option(syntax);
        syntax.InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService>(this ResolverConfig config)
        where TService : class
    {
        config.Bind<TService>().ToSelf().InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService>(
        this ResolverConfig config,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToSelf();
        option(syntax);
        syntax.InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer(
        this ResolverConfig config,
        Type serviceType,
        object value)
    {
        config.Bind(serviceType).ToConstant(value).InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer(
        this ResolverConfig config,
        Type serviceType,
        object value,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToConstant(value);
        option(syntax);
        syntax.InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService>(
        this ResolverConfig config,
        TService value)
        where TService : class
    {
        config.Bind<TService>().ToConstant(value).InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService>(
        this ResolverConfig config,
        TService value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService, TImplementation>(
        this ResolverConfig config,
        TImplementation value)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToConstant(value).InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService, TImplementation>(
        this ResolverConfig config,
        TImplementation value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer(
        this ResolverConfig config,
        Type serviceType,
        Func<IResolver, object> factory)
    {
        config.Bind(serviceType).ToMethod(factory).InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer(
        this ResolverConfig config,
        Type serviceType,
        Func<IResolver, object> factory,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToMethod(factory);
        option(syntax);
        syntax.InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService>(
        this ResolverConfig config,
        Func<IResolver, TService> factory)
        where TService : class
    {
        config.Bind<TService>().ToMethod(factory).InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService>(
        this ResolverConfig config,
        Func<IResolver, TService> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService, TImplementation>(
        this ResolverConfig config,
        Func<IResolver, TImplementation> factory)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToMethod(factory).InContainerScope();
        return config;
    }

    public static ResolverConfig BindContainer<TService, TImplementation>(
        this ResolverConfig config,
        Func<IResolver, TImplementation> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InContainerScope();
        return config;
    }

    // -----------------------------------------------------------
    // Container Keyed
    // ------------------------------------------------------------

    public static ResolverConfig BindKeyedContainer(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Type implementationType)
    {
        config.Bind(serviceType).To(implementationType).InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Type implementationType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).To(implementationType);
        option(syntax);
        syntax.InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType)
    {
        config.Bind(serviceType).ToSelf().InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToSelf();
        option(syntax);
        syntax.InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().To<TImplementation>().InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().To<TImplementation>();
        option(syntax);
        syntax.InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService>(
        this ResolverConfig config,
        object? serviceKey)
        where TService : class
    {
        config.Bind<TService>().ToSelf().InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService>(
        this ResolverConfig config,
        object? serviceKey,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToSelf();
        option(syntax);
        syntax.InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        object value)
    {
        config.Bind(serviceType).ToConstant(value).InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        object value,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToConstant(value);
        option(syntax);
        syntax.InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService>(
        this ResolverConfig config,
        object? serviceKey,
        TService value)
        where TService : class
    {
        config.Bind<TService>().ToConstant(value).InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService>(
        this ResolverConfig config,
        object? serviceKey,
        TService value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        TImplementation value)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToConstant(value).InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        TImplementation value,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToConstant(value);
        option(syntax);
        syntax.InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Func<IResolver, object> factory)
    {
        config.Bind(serviceType).ToMethod(factory).InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer(
        this ResolverConfig config,
        object? serviceKey,
        Type serviceType,
        Func<IResolver, object> factory,
        Action<IBindingConstraintWithSyntax> option)
    {
        var syntax = config.Bind(serviceType).ToMethod(factory);
        option(syntax);
        syntax.InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TService> factory)
        where TService : class
    {
        config.Bind<TService>().ToMethod(factory).InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TService> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TImplementation> factory)
        where TService : class
        where TImplementation : class, TService
    {
        config.Bind<TService>().ToMethod(factory).InContainerScope().Keyed(serviceKey);
        return config;
    }

    public static ResolverConfig BindKeyedContainer<TService, TImplementation>(
        this ResolverConfig config,
        object? serviceKey,
        Func<IResolver, TImplementation> factory,
        Action<IBindingConstraintWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InContainerScope().Keyed(serviceKey);
        return config;
    }
}
