# Smart.Resolver .NET - resolver library for .NET

## What is this?

Smart.Resolver .NET is simplified resolver library, degradation version of Ninject.

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
var resolver = new StandardResolver();

resolver.Bind<IService>().To<Service>().InSingletonScope();
resolver.Bind<Controller>().ToSelf();

var controller = resolver.Get<Controller>();
```

## Bindings

Supported binding syntax.

* Bind
* To

```csharp
// Type IService to Service Type instance
resolver.Bind<IService>().To<Service>();
```

* ToSelf

```csharp
// Type Controller to Controller Type instance
resolver.Bind<Controller>().ToSelf();
```

* ToMethod

```csharp
// Type IScheduler to factory method
resolver.Bind<IScheduler>().ToMethod(x => x.Get<ISchedulerFactory>().GetScheduler());
```

* ToConstant

```csharp
// Type Messenger to instance
resolver.Bind<Messenger>().ToConstant(Messenger.Default);
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
resolver.Bind<TransientObject>().ToSelf().InTransientScope();
```
or
```csharp
resolver.Bind<TransientObject>().ToSelf();
```

### Singleton

* Single instance created and same instance returned
* Lifecycle managed by resolver (IScopeStorage) and Dispose called when resolver disposed

```csharp
resolver.Bind<SingletonObject>().ToSelf().InSingletonScope();
```

### Custom

* You can create a custom scope

```csharp
resolver.Bind<CustomeScopeObject>().ToSelf().InScope(new CustomeScope());
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
resolver.Bind<Child>().ToSelf().InSingletonScope().Named("foo");
resolver.Bind<Child>().ToSelf().InSingletonScope().Named("bar");
resolver.Bind<Parent>().ToSelf();

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
resolver.Bind<ITimer>().To<Timer>().InSingletonScope();
resolver.Bind<Sceduler>().ToSelf().InSingletonScope().WithConstructorArgument("timeout", 30);
```

## Configuration

StandardResolver is constructed from sub-components.
Change the sub-components in the Configure method, can be customized StandardResolver.

```csharp
// Add custom acitivator to pipeline
public class CustomInitializeActivator : IActivator
{
    public void Activate(object instance)
    {
...
    }
}

resolver.Configure(c => c.Get<IActivatePipeline>().Activators.Add(new CustomInitializeActivator()));
```

```csharp
// Disable inject pipeline (property injection disabled)
resolver.Configure(c => c.Remove<IInjectPipeline>());
```

```csharp
// Add custome scope and storage
public class CustomScopeStorage : IScopeStorage
{
...
}

public class CustomScope : IScope
{
    public IScopeStorage GetStorage(IKernel kernel)
    {
        return kernel.Components.Get<CustomScopeStorage>();
    }
}

resolver.Configure(x => x.Register(new CustomScopeStorage()));
resolver.Bind<SimpleObject>().ToSelf().InScope(new CustomScope());
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
resolver.Bind<InitializableObject>().ToSelf().InSingletonScope();

var obj = resolver.Get<InitializableObject>();

Debug.Assert(obj.Initialized);
```

### Constraint

If custom constraints want is as follows:

```csharp
// Create IConstraint implement
public class HasMetadataConstraint : IConstraint
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

resolver.Bind<Child>().ToSelf().InSingletonScope();
resolver.Bind<Child>().ToSelf().InSingletonScope().WithMetadata("hoge", null);
resolver.Bind<Parent>().ToSelf();
```


## Unsupported

* AOP( ﾟдﾟ)､ﾍﾟｯ
* Method Injection (I don't need)
* Request scope (Not supported by the standard)
* Circular reference detection (Your design bug)
