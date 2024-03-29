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

``` ini

BenchmarkDotNet=v0.13.2, OS=Windows 11 (10.0.22621.819)
AMD Ryzen 9 5900X, 1 CPU, 24 logical and 12 physical cores
.NET SDK=7.0.100
  [Host]    : .NET 7.0.0 (7.0.22.51805), X64 RyuJIT AVX2
  MediumRun : .NET 6.0.11 (6.0.1122.52304), X64 RyuJIT AVX2

Job=MediumRun  Jit=RyuJit  Platform=X64  
IterationCount=15  LaunchCount=2  WarmupCount=10  

```
|            Method |  Runtime |       Mean |     Error |    StdDev |     Median |        Min |        Max |        P90 |   Gen0 | Allocated |
|------------------ |--------- |-----------:|----------:|----------:|-----------:|-----------:|-----------:|-----------:|-------:|----------:|
|         Singleton | .NET 6.0 |   3.989 ns | 0.0113 ns | 0.0162 ns |   3.984 ns |   3.962 ns |   4.028 ns |   4.008 ns |      - |         - |
|         Transient | .NET 6.0 |  14.193 ns | 0.2009 ns | 0.2945 ns |  14.327 ns |  13.796 ns |  14.576 ns |  14.539 ns | 0.0014 |      24 B |
|          Combined | .NET 6.0 |  21.649 ns | 0.0970 ns | 0.1390 ns |  21.649 ns |  21.406 ns |  21.930 ns |  21.821 ns | 0.0014 |      24 B |
|           Complex | .NET 6.0 |  41.789 ns | 0.0964 ns | 0.1382 ns |  41.779 ns |  41.567 ns |  42.171 ns |  41.928 ns | 0.0081 |     136 B |
|          Generics | .NET 6.0 |   4.853 ns | 0.0244 ns | 0.0341 ns |   4.847 ns |   4.809 ns |   4.924 ns |   4.897 ns | 0.0014 |      24 B |
| MultipleSingleton | .NET 6.0 |   3.032 ns | 0.0063 ns | 0.0093 ns |   3.031 ns |   3.020 ns |   3.053 ns |   3.044 ns |      - |         - |
| MultipleTransient | .NET 6.0 |  90.568 ns | 0.7225 ns | 1.0591 ns |  90.607 ns |  88.822 ns |  92.202 ns |  91.878 ns | 0.0110 |     184 B |
|            AspNet | .NET 6.0 | 128.502 ns | 1.7733 ns | 2.4860 ns | 129.304 ns | 125.074 ns | 132.817 ns | 131.384 ns | 0.0153 |     256 B |
|         Singleton | .NET 7.0 |   3.698 ns | 0.0511 ns | 0.0716 ns |   3.661 ns |   3.646 ns |   3.861 ns |   3.838 ns |      - |         - |
|         Transient | .NET 7.0 |  13.060 ns | 0.6737 ns | 0.9875 ns |  12.448 ns |  11.945 ns |  14.227 ns |  14.113 ns | 0.0014 |      24 B |
|          Combined | .NET 7.0 |  21.610 ns | 0.0730 ns | 0.1047 ns |  21.633 ns |  21.423 ns |  21.861 ns |  21.715 ns | 0.0014 |      24 B |
|           Complex | .NET 7.0 |  36.072 ns | 0.2061 ns | 0.3084 ns |  36.024 ns |  35.515 ns |  36.785 ns |  36.525 ns | 0.0081 |     136 B |
|          Generics | .NET 7.0 |   4.530 ns | 0.0246 ns | 0.0352 ns |   4.527 ns |   4.438 ns |   4.620 ns |   4.572 ns | 0.0014 |      24 B |
| MultipleSingleton | .NET 7.0 |   3.259 ns | 0.2345 ns | 0.3437 ns |   2.954 ns |   2.925 ns |   3.619 ns |   3.615 ns |      - |         - |
| MultipleTransient | .NET 7.0 |  92.575 ns | 1.5159 ns | 2.2219 ns |  91.501 ns |  89.929 ns |  95.755 ns |  95.100 ns | 0.0109 |     184 B |
|            AspNet | .NET 7.0 | 124.435 ns | 0.5960 ns | 0.8548 ns | 124.224 ns | 122.980 ns | 125.969 ns | 125.535 ns | 0.0153 |     256 B |

## Unsupported

* AOP( ﾟдﾟ)､ﾍﾟｯ
* Method Injection (I don't need but it is possible to cope)
* Circular reference detection (Your design bug)
