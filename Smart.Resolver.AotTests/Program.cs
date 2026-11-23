using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using Smart.Resolver;
using Smart.Resolver.AotTests;

var failures = 0;

void Assert(bool condition, string name)
{
    if (condition)
    {
        Console.WriteLine($"OK   : {name}");
    }
    else
    {
        Console.WriteLine($"FAIL : {name}");
        failures++;
    }
}

{
    var config = new ResolverConfig();
    config.Bind<SelfTarget>().ToSelf().InSingletonScope();
    using var resolver = config.ToResolver();
    var first = resolver.Get<SelfTarget>();
    var second = resolver.Get<SelfTarget>();
    Assert(ReferenceEquals(first, second), "Bind<T>().ToSelf()");
}

{
    var config = new ResolverConfig();
    config.Bind<ConstructorDependency>().ToSelf().InSingletonScope();
    config.Bind<IConstructorService>().To<ConstructorService>();
    using var resolver = config.ToResolver();
    var service = resolver.Get<IConstructorService>() as ConstructorService;
    Assert(ReferenceEquals(service?.Dependency, resolver.Get<ConstructorDependency>()), "Bind<T>().To<TImplementation>() with constructor injection");
}

{
    var serviceType = typeof(ITypeService);
    var implementationType = typeof(TypeService);
    var config = new ResolverConfig();
    config.Bind(serviceType).To(implementationType);
    using var resolver = config.ToResolver();
    Assert(resolver.Get<ITypeService>() is TypeService, "Bind(Type).To(Type)");
}

{
    var serviceType = typeof(ITypeSingletonService);
    var implementationType = typeof(TypeSingletonService);
    var config = new ResolverConfig();
    config.BindSingleton(serviceType, implementationType);
    using var resolver = config.ToResolver();
    var first = resolver.Get<ITypeSingletonService>();
    var second = resolver.Get<ITypeSingletonService>();
    Assert(ReferenceEquals(first, second), "BindSingleton(Type, Type)");
}

{
    var config = new ResolverConfig();
    config.BindSingleton<IGenericSingletonService, GenericSingletonService>();
    using var resolver = config.ToResolver();
    Assert(resolver.Get<IGenericSingletonService>() is GenericSingletonService, "BindSingleton<TService, TImplementation>()");
}

{
    var config = new ResolverConfig().UseProcessor<CustomProcessor>();
    config.Bind<ProcessedTarget>().ToSelf();
    using var resolver = config.ToResolver();
    Assert(resolver.Get<ProcessedTarget>().Processed, "UseProcessor<T>()");
}

{
    var config = new ResolverConfig().UseInitializeProcessor();
    config.Bind<InitializableTarget>().ToSelf();
    using var resolver = config.ToResolver();
    Assert(resolver.Get<InitializableTarget>().Initialized, "UseInitializeProcessor()");
}

{
    var config = new ResolverConfig().UsePropertyInjector();
    config.Bind<PropertyDependency>().ToSelf().InSingletonScope();
    config.Bind<PropertyTarget>().ToSelf();
    using var resolver = config.ToResolver();
    var target = resolver.Get<PropertyTarget>();
    Assert(target.Public is not null, "[Inject] public property");
    Assert(target.PrivateSetter is not null, "[Inject] property with private setter");
    Assert(target.Internal is not null, "[Inject] internal property");
    Assert(target.PrivateValue is not null, "[Inject] private property");
    Assert(target.ProtectedValue is not null, "[Inject] protected property of base class");
}

{
    var config = new ResolverConfig().UsePropertyInjector();
    config.Bind<InjectDependency>().ToSelf().InSingletonScope();
    using var resolver = config.ToResolver();
    var target = new InjectTarget();
    resolver.Inject(target);
    Assert((target.Public is not null) && (target.PrivateValue is not null) && (target.ProtectedValue is not null), "Inject<T>(T)");
    var derived = new InjectDerivedTarget();
    resolver.Inject(derived);
    Assert((derived.Derived is not null) && (derived.Public is not null) && (derived.ProtectedValue is not null), "Inject<T>(T) with the derived type");
    var baseTarget = new InjectDerivedTarget();
    InjectTarget asBase = baseTarget;
    resolver.Inject(asBase);
    Assert((baseTarget.Derived is null) && (baseTarget.Public is not null), "Inject<T>(T) uses the properties of T");
    var viaInterface = new InjectTarget();
    resolver.Get<IResolver>().Inject(viaInterface);
    Assert(viaInterface.Public is not null, "IResolver.Inject<T>(T)");
    using var child = resolver.CreateChildResolver();
    var viaChild = new InjectTarget();
    child.Inject(viaChild);
    Assert(viaChild.Public is not null, "SmartChildResolver.Inject<T>(T)");
}

{
    var methodTarget = new MethodTarget();
    var config = new ResolverConfig();
    config.Bind<MethodTarget>().ToMethod(_ => methodTarget);
    config.Bind<MethodValue>().ToMethod(static _ => new MethodValue(1));
    config.Bind<int?>().ToMethod(static _ => 2);
    using var resolver = config.ToResolver();
    Assert(ReferenceEquals(resolver.Get<MethodTarget>(), methodTarget), "ToMethod() class");
    Assert(resolver.Get<MethodValue>().Value == 1, "ToMethod() struct");
    Assert(resolver.Get<int?>() == 2, "ToMethod() nullable value type");
}

{
    var constant = new ConstantTarget();
    var config = new ResolverConfig();
    config.Bind<ConstantTarget>().ToConstant(constant);
    using var resolver = config.ToResolver();
    Assert(ReferenceEquals(resolver.Get<ConstantTarget>(), constant), "ToConstant()");
}

{
    var config = new ResolverConfig();
    config.Bind<IKeyedService>().To<KeyedServiceA>().InSingletonScope().Keyed("a");
    config.Bind<IKeyedService>().To<KeyedServiceB>().InSingletonScope().Keyed("b");
    config.Bind<KeyedConsumer>().ToSelf();
    using var resolver = config.ToResolver();
    Assert(resolver.Get<KeyedConsumer>().Service is KeyedServiceB, "Keyed() and [ResolveBy]");
}

{
    var config = new ResolverConfig();
    config.Bind<IMultipleService>().To<MultipleServiceA>();
    config.Bind<IMultipleService>().To<MultipleServiceB>();
    config.Bind<int>().ToConstant(1);
    config.Bind<int>().ToConstant(2);
    config.Bind<ArrayValue>().ToConstant(new ArrayValue(1));
    config.Bind<ArrayValue>().ToConstant(new ArrayValue(2));
    config.Bind<MultipleConsumer>().ToSelf();
    using var resolver = config.ToResolver();
    var consumer = resolver.Get<MultipleConsumer>();
    Assert(consumer.ServiceCount == 2, "IEnumerable<T> constructor parameter");
    Assert(consumer.ArrayServiceCount == 2, "T[] constructor parameter");
    Assert(consumer.ValueSum == 3, "IEnumerable<int> constructor parameter");
    Assert(consumer.ArrayValueCount == 2, "struct array constructor parameter");
}

{
    var config = new ResolverConfig().UseArrayBinding();
    config.Bind<IArrayBindingService>().To<ArrayBindingServiceA>();
    config.Bind<IArrayBindingService>().To<ArrayBindingServiceB>();
    using var resolver = config.ToResolver();
    Assert(resolver.Get<IEnumerable<IArrayBindingService>>().Count() == 2, "UseArrayBinding()");
}

{
    var config = new ResolverConfig();
    config.Bind<ScopedTarget>().ToSelf().InContainerScope();
    using var resolver = config.ToResolver();
    using var child = resolver.CreateChildResolver();
    var first = child.Get<ScopedTarget>();
    var second = child.Get<ScopedTarget>();
    Assert(ReferenceEquals(first, second), "child resolver and container scope");
}

{
    var config = new ResolverConfig();
    config.Bind<ConstructorDependency>().ToSelf();
    config.Bind<SelectedConstructorTarget>().ToSelf();
    using var resolver = config.ToResolver();
    var target = resolver.Get<SelectedConstructorTarget>();
    Assert((target.Dependency is not null) && (target.Number == 7), "[Inject] constructor and default value");
}

{
    var config = new ResolverConfig();
    config.Bind<ConstructorDependency>().ToSelf().InSingletonScope();
    config.Bind<FiveArgumentsTarget>().ToSelf();
    using var resolver = config.ToResolver();
    Assert(ReferenceEquals(resolver.Get<FiveArgumentsTarget>().Last, resolver.Get<ConstructorDependency>()), "constructor with five arguments");
}

{
    var config = new ResolverConfig().UseOpenGenericBinding();
    config.Bind<GenericArgument>().ToSelf().InSingletonScope();
    config.Bind<long>().ToConstant(3L);
    config.Bind(typeof(IGenericService<>)).To(typeof(GenericService<>));
    using var resolver = config.ToResolver();
    Assert(resolver.Get<IGenericService<GenericArgument>>() is GenericService<GenericArgument>, "open generic with reference type argument");
    Assert(new GenericService<long>(0).Value == 0, "closed value type is used in code");
    Assert(resolver.Get<IGenericService<long>>() is GenericService<long> { Value: 3 }, "open generic with value type argument used in code");
}

{
    var config = new ResolverConfig().UseAssignableBinding();
    config.Bind<AssignableService>().ToSelf();
    using var resolver = config.ToResolver();
    Assert(resolver.Get<IAssignableService>() is AssignableService, "UseAssignableBinding()");
}

{
    var services = new ServiceCollection();
    services.AddTransient<IMediTransientService, MediTransientService>();
    services.AddSingleton<IMediSingletonService, MediSingletonService>();
    services.AddScoped<IMediScopedService, MediScopedService>();
    services.AddSingleton<IMediFactoryService>(static _ => new MediFactoryService());
    services.AddSingleton<IMediInstanceService>(MediInstanceService.Instance);
    services.AddTransient<MediConsumer>();
    services.AddSingleton<IMediMultipleService, MediMultipleServiceA>();
    services.AddSingleton<IMediMultipleService, MediMultipleServiceB>();
    services.AddSingleton<MediGenericArgument>();
    services.AddSingleton(typeof(long), 3L);
    services.AddSingleton(typeof(IMediGenericService<>), typeof(MediGenericService<>));
    services.AddKeyedSingleton<IMediKeyedService, MediKeyedServiceA>("a");
    services.AddKeyedSingleton<IMediKeyedService, MediKeyedServiceB>("b");
    services.AddTransient<MediKeyedConsumer>();
    services.AddKeyedTransient<MediServiceKeyTarget>("k");
    services.AddScoped<MediDisposableService>();
    services.AddOptions<MediOptions>().Configure(static x => x.Value = 5);
    services.AddLogging();

    var factory = new SmartServiceProviderFactory();
    var provider = factory.CreateServiceProvider(factory.CreateBuilder(services));
    var transient1 = provider.GetRequiredService<IMediTransientService>();
    var transient2 = provider.GetRequiredService<IMediTransientService>();
    Assert(!ReferenceEquals(transient1, transient2), "IServiceCollection transient");
    var singleton1 = provider.GetRequiredService<IMediSingletonService>();
    var singleton2 = provider.GetRequiredService<IMediSingletonService>();
    Assert(ReferenceEquals(singleton1, singleton2), "IServiceCollection singleton");
    using (var scope1 = provider.CreateScope())
    using (var scope2 = provider.CreateScope())
    {
        var first = scope1.ServiceProvider.GetRequiredService<IMediScopedService>();
        Assert(ReferenceEquals(first, scope1.ServiceProvider.GetRequiredService<IMediScopedService>()) && !ReferenceEquals(first, scope2.ServiceProvider.GetRequiredService<IMediScopedService>()), "IServiceCollection scoped");
    }

    Assert(provider.GetRequiredService<IMediFactoryService>() is MediFactoryService, "IServiceCollection factory");
    Assert(ReferenceEquals(provider.GetRequiredService<IMediInstanceService>(), MediInstanceService.Instance), "IServiceCollection instance");
    Assert(provider.GetRequiredService<MediConsumer>().Singleton is MediSingletonService, "IServiceCollection constructor injection");
    Assert(provider.GetServices<IMediMultipleService>().Count() == 2, "IServiceCollection IEnumerable<T>");
    Assert(provider.GetRequiredService<IMediGenericService<MediGenericArgument>>() is MediGenericService<MediGenericArgument>, "IServiceCollection open generic with reference type argument");
    Assert(new MediGenericService<long>(0).Value == 0, "IServiceCollection closed value type is used in code");
    Assert(provider.GetRequiredService<IMediGenericService<long>>() is MediGenericService<long> { Value: 3 }, "IServiceCollection open generic with value type argument used in code");
    Assert(provider.GetRequiredKeyedService<IMediKeyedService>("b") is MediKeyedServiceB, "IServiceCollection keyed service");
    Assert(provider.GetRequiredService<MediKeyedConsumer>().Service is MediKeyedServiceA, "[FromKeyedServices]");
    Assert(provider.GetRequiredKeyedService<MediServiceKeyTarget>("k").Key == "k", "[ServiceKey]");
    Assert(provider.GetRequiredService<IServiceProviderIsService>().IsService(typeof(IMediSingletonService)), "IServiceProviderIsService");
    MediDisposableService disposable;
    using (var scope = provider.CreateScope())
    {
        disposable = scope.ServiceProvider.GetRequiredService<MediDisposableService>();
    }

    Assert(disposable.Disposed, "IServiceCollection scoped disposal");
    Assert(provider.GetRequiredService<IOptions<MediOptions>>().Value.Value == 5, "IOptions<T>");
    Assert(provider.GetRequiredService<ILogger<MediConsumer>>() is Logger<MediConsumer>, "ILogger<T>");
    ((IDisposable)provider).Dispose();
}

{
    var builder = Host.CreateApplicationBuilder();
    builder.Logging.ClearProviders();
    builder.Services.AddHostedService<MediHostedService>();
    builder.Services.AddOptions<MediHostOptions>().Configure(static x => x.Value = 7);
    builder.ConfigureContainer(new SmartServiceProviderFactory());
    using var host = builder.Build();
    await host.StartAsync().ConfigureAwait(false);
    var service = host.Services.GetServices<IHostedService>().OfType<MediHostedService>().Single();
    var value = host.Services.GetRequiredService<IOptions<MediHostOptions>>().Value.Value;
    await host.StopAsync().ConfigureAwait(false);
    Assert(service.Started && service.Stopped && (value == 7), "Generic Host with SmartServiceProviderFactory");
}

Console.WriteLine(failures == 0 ? "ALL OK" : $"FAILURES={failures}");
return failures == 0 ? 0 : 1;
