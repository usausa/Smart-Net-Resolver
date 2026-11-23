# Smart.Resolver .NET - resolver library for .NET

[![NuGet](https://img.shields.io/nuget/v/Usa.Smart.Resolver.svg)](https://www.nuget.org/packages/Usa.Smart.Resolver)

## What is this?

Smart.Resolver .NET is simplified resolver library, degradation version of Ninject.

* ASP.NET Core / Generic Host support
* Native AOT support
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

`Inject<T>(T)` injects the properties of `T`, and `Inject(object)` injects the properties of the runtime type.

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

## Native AOT

Smart.Resolver and the extensions are trimming and Native AOT compatible.
Bindings registered with `Bind<T>()` or `Bind(typeof(T))` and services registered with `IServiceCollection` work without additional settings.

### APIs requiring unreferenced code

The following APIs resolve types at runtime, and are marked with `RequiresUnreferencedCode` (and `RequiresDynamicCode`).

| API | Alternative |
|-|-|
| `UseAutoBinding()` | Bind the types with `Bind<T>()`, or keep the constructors with `[DynamicDependency]` |
| `Inject(object)` | Pass the instance as its class type to call `Inject<T>(T)`, which injects the properties of `T` |
| `LoadXml()` / `LoadXmlFile()` / `LoadDefinition()` | Keep the constructors of the types in the definition with `[DynamicDependency]` |
| `LoadJson()` / `LoadJsonFile()` | Deserialize `ResolverDefinition` with a source generated `JsonSerializerContext`, and use `LoadDefinition()` |
| `BindConfig<TConfig>()` | Use `configuration.Bind(instance)` in the application, and register the instance with `ToConstant()` |

Warnings at the call sites remain. Suppress them after keeping the members.

```csharp
[DynamicDependency(DynamicallyAccessedMemberTypes.PublicConstructors, typeof(Service))]
[UnconditionalSuppressMessage("Trimming", "IL2026", Justification = "Constructors are kept by DynamicDependency.")]
private static SmartResolver CreateResolver()
{
    var config = new ResolverConfig();
    config.UseAutoBinding();
    return config.ToResolver();
}
```

```csharp
// Source generated context for definitions
[JsonSourceGenerationOptions(PropertyNameCaseInsensitive = true, UseStringEnumConverter = true)]
[JsonSerializable(typeof(ResolverDefinition))]
internal sealed partial class DefinitionContext : JsonSerializerContext;

var definition = JsonSerializer.Deserialize(json, DefinitionContext.Default.ResolverDefinition)!;
config.LoadDefinition(definition);
```

### Value types

* Open generic bindings with value type arguments need native code for the closed type. Bind the closed type explicitly (e.g. `config.Bind<IGenericService<int>>().To<GenericService<int>>()`)
* `IEnumerable<T>` of value types needs native code for `T[]`. Use `T[]` instead (e.g. `resolver.Get<Point[]>()` or a `Point[]` constructor parameter)

### Property injection

* Property injection for services registered with `IServiceCollection` needs the properties kept with `[DynamicDependency]`

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
BenchmarkDotNet v0.16.0-preview.2, Windows 11 (10.0.26300.10075)
AMD Ryzen 9 5900X 3.69GHz, 1 CPU, 24 logical and 12 physical cores
Memory: 63.93 GB Total, 39.53 GB Available
.NET SDK 11.0.100
  [Host]    : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v3
  MediumRun : .NET 11.0.0 (11.0.0, 11.0.26.53018), X64 RyuJIT x86-64-v3

Job=MediumRun  Jit=RyuJit  Platform=X64  
IterationCount=15  LaunchCount=6  WarmupCount=10  
```
| Method            | Mean      | Error     | StdDev    | Median    | Min       | Max       | P90       | Gen0   | Allocated |
|------------------ |----------:|----------:|----------:|----------:|----------:|----------:|----------:|-------:|----------:|
| Singleton         |  1.423 ns | 0.0067 ns | 0.0183 ns |  1.422 ns |  1.394 ns |  1.469 ns |  1.449 ns |      - |         - |
| Transient         |  3.615 ns | 0.0394 ns | 0.1084 ns |  3.590 ns |  3.473 ns |  3.973 ns |  3.770 ns | 0.0011 |      19 B |
| Combined          |  4.740 ns | 0.0640 ns | 0.1764 ns |  4.680 ns |  4.520 ns |  5.127 ns |  5.024 ns | 0.0014 |      24 B |
| Complex           | 17.952 ns | 0.1276 ns | 0.3535 ns | 17.914 ns | 17.320 ns | 18.870 ns | 18.384 ns | 0.0081 |     136 B |
| Generics          |  3.013 ns | 0.0580 ns | 0.1599 ns |  2.980 ns |  2.786 ns |  3.395 ns |  3.255 ns | 0.0006 |      10 B |
| MultipleSingleton |  1.313 ns | 0.0062 ns | 0.0170 ns |  1.308 ns |  1.291 ns |  1.363 ns |  1.336 ns |      - |         - |
| MultipleTransient | 17.602 ns | 0.1158 ns | 0.3210 ns | 17.542 ns | 17.052 ns | 18.466 ns | 18.044 ns | 0.0110 |     184 B |
| AspNet            | 62.470 ns | 1.7136 ns | 4.6328 ns | 61.245 ns | 57.978 ns | 74.787 ns | 72.320 ns | 0.0119 |     200 B |

## Unsupported

* AOP( ﾟдﾟ)､ﾍﾟｯ
* Method Injection (I don't need but it is possible to cope)
* Circular reference detection (Your design bug)
