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

public class Service : IService
{
}

public class Controller
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
public class Child
{
}

public class Parent
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
public class HasPropertyObject
{
    [Inject]
    public Target Target { get; set; }
}
```

## Parameter

Set constructor argument or property value.

```csharp
public class Sceduler
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
public class Startup
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
public class Parent
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
BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19041.450 (2004/?/20H1)
Intel Core i5-9500 CPU 3.00GHz, 1 CPU, 6 logical and 6 physical cores
.NET Core SDK=5.0.100-preview.7.20366.6
  [Host]     : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT
  DefaultJob : .NET Core 3.1.6 (CoreCLR 4.700.20.26901, CoreFX 4.700.20.31603), X64 RyuJIT
```
|            Method |       Mean |     Error |    StdDev |     Median |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------ |-----------:|----------:|----------:|-----------:|-------:|------:|------:|----------:|
|         Singleton |   6.676 ns | 0.0286 ns | 0.0267 ns |   6.676 ns |      - |     - |     - |         - |
|         Transient |   9.649 ns | 0.1937 ns | 0.2651 ns |   9.816 ns | 0.0051 |     - |     - |      24 B |
|          Combined |  13.453 ns | 0.2556 ns | 0.2391 ns |  13.558 ns | 0.0051 |     - |     - |      24 B |
|           Complex |  52.438 ns | 0.1961 ns | 0.1834 ns |  52.462 ns | 0.0289 |     - |     - |     136 B |
|          Generics |   8.772 ns | 0.0614 ns | 0.0575 ns |   8.782 ns | 0.0051 |     - |     - |      24 B |
| MultipleSingleton |   6.133 ns | 0.0244 ns | 0.0228 ns |   6.125 ns |      - |     - |     - |         - |
| MultipleTransient |  62.597 ns | 0.2012 ns | 0.1783 ns |  62.533 ns | 0.0391 |     - |     - |     184 B |
|            AspNet | 174.787 ns | 0.4155 ns | 0.3683 ns | 174.781 ns | 0.0544 |     - |     - |     256 B |

## Unsupported

* AOP( ﾟдﾟ)､ﾍﾟｯ
* Method Injection (I don't need but it is possible to cope)
* Circular reference detection (Your design bug)
