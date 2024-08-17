# Smart.Resolver .NET - resolver library for .NET

[![NuGet Badge](https://buildstats.info/nuget/Usa.Smart.Resolver)](https://www.nuget.org/packages/Usa.Smart.Resolver/)

## What is this?

Smart.Resolver .NET is simplified resolver library, degradation version of Ninject.

* ASP.NET Core / Generic Host support
* Xamarin support (Code generation mode and Reflection mode both supported)
* Transient, Singleton, Container(child) and custom scope supported
* Callback, Constant provider supported
* Property injection supported (optional)
* Custom initialize processor supported
* Construct with parameter supported
* Constraint supported (like named)
* Missing handler supported (For automatic registration, Xamarin integration, open generic type, ...)
* Customization-first implementation, but not too late (see benchmark)

### Usage example

```csharp
public interface IService
{
}

public sealed class Service : IService
{
}

public sealed class Controller
{
    private IService Service { get; }

    public Controller(IService service)
    {
        Service = service;
    }
}

// Usage 
var config = new ResolverConfig();
config.Bind<IService>().To<Service>().InSingletonScope();
config.Bind<Controller>().ToSelf();

var resolver = config.ToResolver();

var controller = resolver.Get<Controller>();
```

## NuGet

| Package | Note  |
|-|-|
| [![NuGet Badge](https://buildstats.info/nuget/Usa.Smart.Resolver)](https://www.nuget.org/packages/Usa.Smart.Resolver/) | Core libyrary  |
| [![NuGet Badge](https://buildstats.info/nuget/Usa.Smart.Resolver.Extensions.DependencyInjection)](https://www.nuget.org/packages/Usa.Smart.Resolver.Extensions.DependencyInjection/) | Microsoft.Extensions.DependencyInjection integration |
| [![NuGet Badge](https://buildstats.info/nuget/Usa.Smart.Resolver.Xamarin)](https://www.nuget.org/packages/Usa.Smart.Resolver.Xamarin/) | Xamarin DependencyService integration |

## Bindings

Supported binding syntax.

* Bind
* To

```csharp
// Type IService to Service Type instance
config.Bind<IService>().To<Service>();
```

* ToSelf

```csharp
// Type Controller to Controller Type instance
config.Bind<Controller>().ToSelf();
```

* ToMethod

```csharp
// Type IScheduler to factory method
config.Bind<IScheduler>().ToMethod(x => x.Get<ISchedulerFactory>().GetScheduler());
```

* ToConstant

```csharp
// Type Messenger to instance
config.Bind<Messenger>().ToConstant(Messenger.Default);
```

* InTransientScope
* InSingletonScope
* InScope
* Named
* WithConstructorArgument
* WithPropertyValue
* WithMetadata

## Scope

Supported scope.

### Transient (default)

* New instance created each time
* Lifecycle is not managed by resolver

```csharp
config.Bind<TransientObject>().ToSelf().InTransientScope();
```
or
```csharp
config.Bind<TransientObject>().ToSelf();
```

### Singleton

* Single instance created and same instance returned
* Lifecycle managed by resolver (IScopeStorage) and Dispose called when resolver disposed

```csharp
config.Bind<SingletonObject>().ToSelf().InSingletonScope();
```

### Container

* Single instance created and same instance returned per child container
* Lifecycle managed by child container and Dispose called when resolver disposed

```csharp
config.Bind<ScopeObject>().ToSelf().InContainerScope();
```

### Custom

* You can create a custom scope

```csharp
config.Bind<CustomeScopeObject>().ToSelf().InScope(new CustomeScope());
```

## Attribute

Prepared by standard.

### NamedAttribute

Naming constraint for lookup binding.

```csharp
public sealed class Child
{
}

public sealed class Parent
{
    pulbic Child Child { get; }

    public Parent([Named("foo")] Child child)
    {
        Child = child;
    }
}

// Usage
var config = new ResolverConfig();
config.Bind<Child>().ToSelf().InSingletonScope().Named("foo");
config.Bind<Child>().ToSelf().InSingletonScope().Named("bar");
config.Bind<Parent>().ToSelf();

var resolver = config.ToResolver();

var parent = resolver.Get<Parent>();
var foo = resolver.Get<Child>("foo");
var bar = resolver.Get<Child>("bar");

Debug.Assert(parent.Child == foo);
Debug.Assert(parent.Child != bar);
```

### InjectAttribute

Mark of property injection target or select constructor.

```csharp
public sealed class HasPropertyObject
{
    [Inject]
    public Target Target { get; set; }
}
```

## Parameter

Set constructor argument or property value.

```csharp
public sealed class Sceduler
{
    public Sceduler(ITimer timer, int timeout)
    {
    }
}

// Usage
config.Bind<ITimer>().To<Timer>().InSingletonScope();
config.Bind<Sceduler>().ToSelf().InSingletonScope().WithConstructorArgument("timeout", 30);
```

## Configuration

StandardResolver is constructed from sub-components. Change the sub-components in ResolverConfig, can be customized StandardResolver.

```csharp
// Add custom processor to pipeline
public sealed class CustomInitializeProcessor : IProcessor
{
    public void Initialize(object instance)
    {
...
    }
}

config.UseProcessor<CustomInitializeProcessor>();
```

```csharp
// Add custome scope
public sealed class CustomScope : IScope
{
    private static readonly ThreadLocal<Dictionary<IBinding, object>> Cache =
        new ThreadLocal<Dictionary<IBinding, object>>(() => new Dictionary<IBinding, object>());

    public IScope Copy(IComponentContainer components)
    {
        return this;
    }

    public Func<IResolver, object> Create(IBinding binding, Func<object> factory)
    {
        return resolver =>
        {
            if (Cache.Value.TryGetValue(binding, out var value))
            {
                return value;
            }

            value = factory();
            Cache.Value[binding] = value;

            return value;
        };
    }
}

config.Components.Add<CustomScopeStorage>();
config.Bind<SimpleObject>().ToSelf().InScope(new CustomScope());
```

## Integration

See the sample project for details.

### ASP.NET Core 3.1

```csharp
public static class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .UseServiceProviderFactory(new SmartServiceProviderFactory())
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
}
```

```csharp
public sealed class Startup
{
...
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMvc();
    }

    public void ConfigureContainer(ResolverConfig config)
    {
        // Add component
    }
...
}
```

### Generic Host

```csharp
public static class Program
{
    public static async Task Main(string[] args)
    {
        await new HostBuilder()
            .UseServiceProviderFactory(new SmartServiceProviderFactory())
            .ConfigureContainer<ResolverConfig>(ConfigureContainer)
            .RunAsync();
    }

    private static void ConfigureContainer(ResolverConfig config)
    {
        // Add component
    }
}
```

### Xamarin

```csharp
var config = new ResolverConfig()
    .UseDependencyService();

var resolver = config.ToResolver();
```

## Other

Ohter topics.

### IInitializable

If the class implements Initializable, Initialized called after construct.

```csharp
protected class InitializableObject : IInitializable
{
    public bool Initialized { get; private set; }

    public void Initialize()
    {
        Initialized = true;
    }
}

// Usage
config.Bind<InitializableObject>().ToSelf().InSingletonScope();

var obj = resolver.Get<InitializableObject>();

Debug.Assert(obj.Initialized);
```

### Constraint

If custom constraints want is as follows:

```csharp
// Create IConstraint implement
public sealed class HasMetadataConstraint : IConstraint
{
    public string Key { get; }

    public HasMetadataConstraint(string key)
    {
        Key = key;
    }

    public bool Match(IBindingMetadata metadata)
    {
        return metadata.Has(Key);
    }
}

// Create ConstraintAttribute derived class
public sealed class HasMetadataAttribute : ConstraintAttribute
{
    public string Key { get; }

    public HasMetadataAttribute(string key)
    {
        Key = key;
    }

    public override IConstraint CreateConstraint()
    {
        return new HasMetadataConstraint(Key);
    }
}

// Usage
public sealed class Parent
{
    pulbic Child Child { get; }

    public Parent([HasMetadata("hoge")] Child child)
    {
        Child = child;
    }
}

config.Bind<Child>().ToSelf().InSingletonScope();
config.Bind<Child>().ToSelf().InSingletonScope().WithMetadata("hoge", null);
config.Bind<Parent>().ToSelf();
```

## Benchmark (for reference purpose only)

```
BenchmarkDotNet v0.13.12, Windows 11 (10.0.22631.3958/23H2/2023Update/SunValley3)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK 8.0.400
  [Host]    : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2
  MediumRun : .NET 8.0.8 (8.0.824.36612), X64 RyuJIT AVX2

Job=MediumRun  Jit=RyuJit  Platform=X64  
Runtime=.NET 8.0  IterationCount=15  LaunchCount=2  
WarmupCount=10  
```
| Method            | Mean       | Error     | StdDev    | Min       | Max        | P90        | Gen0   | Allocated |
|------------------ |-----------:|----------:|----------:|----------:|-----------:|-----------:|-------:|----------:|
| Singleton         |   2.258 ns | 0.0221 ns | 0.0331 ns |  2.223 ns |   2.328 ns |   2.313 ns |      - |         - |
| Transient         |  11.712 ns | 0.8301 ns | 1.2424 ns | 10.154 ns |  13.742 ns |  13.439 ns | 0.0014 |      24 B |
| Combined          |  22.017 ns | 0.2220 ns | 0.3322 ns | 21.503 ns |  22.911 ns |  22.385 ns | 0.0014 |      24 B |
| Complex           |  36.063 ns | 0.5290 ns | 0.7754 ns | 35.258 ns |  38.750 ns |  36.957 ns | 0.0081 |     136 B |
| Generics          |   4.113 ns | 0.0513 ns | 0.0720 ns |  3.995 ns |   4.249 ns |   4.194 ns | 0.0014 |      24 B |
| MultipleSingleton |   2.074 ns | 0.0226 ns | 0.0324 ns |  2.033 ns |   2.143 ns |   2.116 ns |      - |         - |
| MultipleTransient |  91.725 ns | 0.8332 ns | 1.2471 ns | 89.852 ns |  94.266 ns |  93.480 ns | 0.0110 |     184 B |
| AspNet            | 101.012 ns | 0.8376 ns | 1.1465 ns | 99.009 ns | 103.599 ns | 102.215 ns | 0.0153 |     256 B |

## Unsupported

* AOP( ﾟдﾟ)､ﾍﾟｯ
* Method Injection (I don't need but it is possible to cope)
* Circular reference detection (Your design bug)
