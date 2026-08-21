# Smart.Resolver .NET - resolver library for .NET

[![NuGet](https://img.shields.io/nuget/v/Usa.Smart.Resolver.svg)](https://www.nuget.org/packages/Usa.Smart.Resolver)

## What is this?

Smart.Resolver .NET is simplified resolver library, degradation version of Ninject.

* ASP.NET Core / Generic Host support
* Transient, Singleton, Container(child) and custom scope supported
* Callback, Constant provider supported
* Property injection supported (optional)
* Custom initialize processor supported
* Construct with parameter supported
* Constraint supported (like keyed)
* Missing handler supported (For automatic registration, open generic type, ...)
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
| [![NuGet Badge](https://img.shields.io/nuget/v/Usa.Smart.Resolver.svg)](https://www.nuget.org/packages/Usa.Smart.Resolver/) | Core libyrary  |
| [![NuGet Badge](https://img.shields.io/nuget/v/Usa.Smart.Resolver.Extensions.DependencyInjection.svg)](https://www.nuget.org/packages/Usa.Smart.Resolver.Extensions.DependencyInjection/) | Microsoft.Extensions.DependencyInjection integration |
| [![NuGet Badge](https://img.shields.io/nuget/v/Usa.Smart.Resolver.Extensions.Configuration.svg)](https://www.nuget.org/packages/Usa.Smart.Resolver.Extensions.Configuration/) | Configuration extension |

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
* Keyed
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
* Lifecycle managed by resolver and Dispose called when resolver disposed

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

### ResolveByAttribute

Key constraint for lookup binding.

```csharp
public sealed class Child
{
}

public sealed class Parent
{
    pulbic Child Child { get; }

    public Parent([ResolveBy("foo")] Child child)
    {
        Child = child;
    }
}

// Usage
var config = new ResolverConfig();
config.Bind<Child>().ToSelf().InSingletonScope().Keyed("foo");
config.Bind<Child>().ToSelf().InSingletonScope().Keyed("bar");
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

## ResolverOption

Optional behaviours, off by default.

```csharp
config.UseOption(new ResolverOption { DisposalTracking = true });
```

### DisposalTracking

* Created IDisposable instances are disposed by the owning resolver or child container in reverse creation order
* Only bindings that can produce IDisposable are affected

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
    private static readonly ThreadLocal<Dictionary<CustomScope, object>> Cache = new(() => []);

    // Return true to hand disposal of created instances to the resolver
    public bool TransferDisposal() => false;

    public IScope Copy(ComponentContainer components)
    {
        return this;
    }

    public Func<IResolver, object> Create(IResolver resolver, Func<IResolver, object> factory)
    {
        return r =>
        {
            if (Cache.Value.TryGetValue(this, out var value))
            {
                return value;
            }

            value = factory(r);
            Cache.Value[this] = value;

            return value;
        };
    }
}

config.Components.Add<CustomScopeStorage>();
config.Bind<SimpleObject>().ToSelf().InScope(new CustomScope());
```

## Integration

See the sample project for details.

### Microsoft.Extensions.DependencyInjection compatibility

See [Smart.Resolver.CompatibilityTest](Smart.Resolver.CompatibilityTest/README.md).

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
BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.9168/25H2/2025Update/HudsonValley2)
AMD Ryzen 9 5900X 3.70GHz, 1 CPU, 24 logical and 12 physical cores
.NET SDK 10.0.400
  [Host]    : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3
  MediumRun : .NET 10.0.11 (10.0.11, 10.0.1126.37416), X64 RyuJIT x86-64-v3

Job=MediumRun  Jit=RyuJit  Platform=X64  
IterationCount=15  LaunchCount=6  WarmupCount=10  
```
| Method            | Mean      | Error     | StdDev    | Median    | Min       | Max       | P90       | Gen0   | Allocated |
|------------------ |----------:|----------:|----------:|----------:|----------:|----------:|----------:|-------:|----------:|
| Singleton         |  1.335 ns | 0.0610 ns | 0.1701 ns |  1.257 ns |  1.181 ns |  1.803 ns |  1.614 ns |      - |         - |
| Transient         |  3.993 ns | 0.0913 ns | 0.2544 ns |  3.980 ns |  3.595 ns |  4.518 ns |  4.356 ns | 0.0011 |      19 B |
| Combined          |  6.646 ns | 0.1274 ns | 0.3552 ns |  6.628 ns |  6.041 ns |  7.542 ns |  7.143 ns | 0.0014 |      24 B |
| Complex           | 28.686 ns | 0.4447 ns | 1.2397 ns | 28.592 ns | 26.525 ns | 31.766 ns | 30.252 ns | 0.0081 |     136 B |
| Generics          |  3.123 ns | 0.0366 ns | 0.1020 ns |  3.114 ns |  2.910 ns |  3.346 ns |  3.248 ns | 0.0006 |      10 B |
| MultipleSingleton |  1.702 ns | 0.0159 ns | 0.0444 ns |  1.691 ns |  1.618 ns |  1.798 ns |  1.756 ns |      - |         - |
| MultipleTransient | 21.142 ns | 0.2722 ns | 0.7587 ns | 21.071 ns | 19.515 ns | 22.827 ns | 21.957 ns | 0.0110 |     184 B |
| AspNet            | 70.618 ns | 0.5556 ns | 1.5303 ns | 70.584 ns | 66.738 ns | 74.371 ns | 72.556 ns | 0.0119 |     200 B |

## Unsupported

* AOP( ﾟдﾟ)､ﾍﾟｯ
* Method Injection (I don't need but it is possible to cope)
* Circular reference detection (Your design bug)
