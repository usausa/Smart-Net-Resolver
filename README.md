# Smart.Resolver .NET - resolver library for .NET

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

| Id                                                | Note                                                 |
|---------------------------------------------------|------------------------------------------------------|
| Usa.Smart.Resolver                                | Core libyrary                                        |
| Usa.Smart.Resolver.Extensions.DependencyInjection | Microsoft.Extensions.DependencyInjection integration |
| Usa.Smart.Resolver.Xamarin                        | Xamarin DependencyService integration                |

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

### ASP.NET Core

```csharp
public static class Program
{
    public static void Main(string[] args)
    {
        CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
            .ConfigureServices(services => services.AddSmartResolver())
            .UseStartup<Startup>();
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

Benchmark result on .NET Core 2.2 with Code generation mode.

|            Method |       Mean |     Error |    StdDev |  Gen 0 | Gen 1 | Gen 2 | Allocated |
|------------------ |-----------:|----------:|----------:|-------:|------:|------:|----------:|
|         Singleton |   8.830 ns | 0.0127 ns | 0.0119 ns |      - |     - |     - |         - |
|         Transient |  10.706 ns | 0.0115 ns | 0.0102 ns | 0.0057 |     - |     - |      24 B |
|          Combined |  14.928 ns | 0.0106 ns | 0.0099 ns | 0.0057 |     - |     - |      24 B |
|           Complex |  47.246 ns | 0.1279 ns | 0.1196 ns | 0.0324 |     - |     - |     136 B |
|          Generics |   9.892 ns | 0.0580 ns | 0.0543 ns | 0.0057 |     - |     - |      24 B |
| MultipleSingleton |   6.989 ns | 0.0108 ns | 0.0101 ns |      - |     - |     - |         - |
| MultipleTransient |  62.508 ns | 0.0715 ns | 0.0634 ns | 0.0438 |     - |     - |     184 B |
|            AspNet | 303.839 ns | 0.2027 ns | 0.1896 ns | 0.1179 |     - |     - |     496 B |

## Unsupported

* AOP( ﾟдﾟ)､ﾍﾟｯ
* Method Injection (I don't need but it is possible to cope)
* Circular reference detection (Your design bug)
