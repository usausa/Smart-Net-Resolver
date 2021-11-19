namespace Smart.Resolver;

using System;

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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InTransientScope();
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InSingletonScope();
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
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
        Action<IBindingNamedWithSyntax> option)
        where TService : class
        where TImplementation : class, TService
    {
        var syntax = config.Bind<TService>().ToMethod(factory);
        option(syntax);
        syntax.InContainerScope();
        return config;
    }
}
