namespace Smart.Resolver.AotTests;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Smart.Resolver.Attributes;
using Smart.Resolver.Processors;

public sealed class SelfTarget;

public sealed class ConstructorDependency;

public interface IConstructorService;

public sealed class ConstructorService : IConstructorService
{
    public ConstructorDependency Dependency { get; }

    public ConstructorService(ConstructorDependency dependency)
    {
        Dependency = dependency;
    }
}

public interface ITypeService;

public sealed class TypeService : ITypeService;

public interface ITypeSingletonService;

public sealed class TypeSingletonService : ITypeSingletonService;

public interface IGenericSingletonService;

public sealed class GenericSingletonService : IGenericSingletonService;

public sealed class ProcessedTarget
{
    public bool Processed { get; set; }
}

public sealed class CustomProcessor : IProcessor
{
    public int Order => 0;

    public Action<IResolver, object>? CreateProcessor(Type type)
    {
        if (type != typeof(ProcessedTarget))
        {
            return null;
        }

        return static (_, x) => ((ProcessedTarget)x).Processed = true;
    }
}

public sealed class InitializableTarget : IInitializable
{
    public bool Initialized { get; private set; }

    public void Initialize() => Initialized = true;
}

public sealed class PropertyDependency;

public class PropertyTargetBase
{
    [Inject]
    protected PropertyDependency? Protected { get; set; }

    public PropertyDependency? ProtectedValue => Protected;
}

public sealed class PropertyTarget : PropertyTargetBase
{
    [Inject]
    public PropertyDependency? Public { get; set; }

    [Inject]
    public PropertyDependency? PrivateSetter { get; private set; }

    [Inject]
    internal PropertyDependency? Internal { get; set; }

    [Inject]
    private PropertyDependency? Private { get; set; }

    public PropertyDependency? PrivateValue => Private;
}

public sealed class InjectDependency;

public class InjectTargetBase
{
    [Inject]
    protected InjectDependency? Protected { get; set; }

    public InjectDependency? ProtectedValue => Protected;
}

public class InjectTarget : InjectTargetBase
{
    [Inject]
    public InjectDependency? Public { get; set; }

    [Inject]
    private InjectDependency? Private { get; set; }

    public InjectDependency? PrivateValue => Private;
}

public sealed class InjectDerivedTarget : InjectTarget
{
    [Inject]
    public InjectDependency? Derived { get; set; }
}

public sealed class MethodTarget;

public readonly record struct MethodValue(int Value);

public sealed class ConstantTarget;

public interface IKeyedService;

public sealed class KeyedServiceA : IKeyedService;

public sealed class KeyedServiceB : IKeyedService;

public sealed class KeyedConsumer
{
    public IKeyedService Service { get; }

    public KeyedConsumer([ResolveBy("b")] IKeyedService service)
    {
        Service = service;
    }
}

public interface IMultipleService;

public sealed class MultipleServiceA : IMultipleService;

public sealed class MultipleServiceB : IMultipleService;

public readonly record struct ArrayValue(int Value);

public sealed class MultipleConsumer
{
    public int ServiceCount { get; }

    public int ArrayServiceCount { get; }

    public int ValueSum { get; }

    public int ArrayValueCount { get; }

    public MultipleConsumer(IEnumerable<IMultipleService> services, IMultipleService[] arrayServices, IEnumerable<int> values, ArrayValue[] arrayValues)
    {
        ServiceCount = services.Count();
        ArrayServiceCount = arrayServices.Length;
        ValueSum = values.Sum();
        ArrayValueCount = arrayValues.Length;
    }
}

public interface IArrayBindingService;

public sealed class ArrayBindingServiceA : IArrayBindingService;

public sealed class ArrayBindingServiceB : IArrayBindingService;

public sealed class ScopedTarget;

public sealed class SelectedConstructorTarget
{
    public ConstructorDependency? Dependency { get; }

    public int Number { get; }

    public SelectedConstructorTarget()
    {
    }

    [Inject]
    public SelectedConstructorTarget(ConstructorDependency dependency, int number = 7)
    {
        Dependency = dependency;
        Number = number;
    }
}

public sealed class FiveArgumentsTarget
{
    public ConstructorDependency Last { get; }

    public FiveArgumentsTarget(ConstructorDependency first, ConstructorDependency second, ConstructorDependency third, ConstructorDependency fourth, ConstructorDependency last)
    {
        _ = (first, second, third, fourth);
        Last = last;
    }
}

public sealed class GenericArgument;

public interface IGenericService<out T>
{
    T Value { get; }
}

public sealed class GenericService<T> : IGenericService<T>
{
    public T Value { get; }

    public GenericService(T value)
    {
        Value = value;
    }
}

public interface IAssignableService;

public sealed class AssignableService : IAssignableService;

public interface IMediTransientService;

public sealed class MediTransientService : IMediTransientService;

public interface IMediSingletonService;

public sealed class MediSingletonService : IMediSingletonService;

public interface IMediScopedService;

public sealed class MediScopedService : IMediScopedService;

public interface IMediFactoryService;

public sealed class MediFactoryService : IMediFactoryService;

public interface IMediInstanceService;

public sealed class MediInstanceService : IMediInstanceService
{
    public static MediInstanceService Instance { get; } = new();
}

public sealed class MediConsumer
{
    public IMediSingletonService Singleton { get; }

    public IMediTransientService Transient { get; }

    public MediConsumer(IMediSingletonService singleton, IMediTransientService transient)
    {
        Singleton = singleton;
        Transient = transient;
    }
}

public interface IMediMultipleService;

public sealed class MediMultipleServiceA : IMediMultipleService;

public sealed class MediMultipleServiceB : IMediMultipleService;

public sealed class MediGenericArgument;

public interface IMediGenericService<out T>
{
    T Value { get; }
}

public sealed class MediGenericService<T> : IMediGenericService<T>
{
    public T Value { get; }

    public MediGenericService(T value)
    {
        Value = value;
    }
}

public interface IMediKeyedService;

public sealed class MediKeyedServiceA : IMediKeyedService;

public sealed class MediKeyedServiceB : IMediKeyedService;

public sealed class MediKeyedConsumer
{
    public IMediKeyedService Service { get; }

    public MediKeyedConsumer([FromKeyedServices("a")] IMediKeyedService service)
    {
        Service = service;
    }
}

public sealed class MediServiceKeyTarget
{
    public string Key { get; }

    public MediServiceKeyTarget([ServiceKey] string key)
    {
        Key = key;
    }
}

public sealed class MediDisposableService : IDisposable
{
    public bool Disposed { get; private set; }

    public void Dispose()
    {
        Disposed = true;
    }
}

public sealed class MediOptions
{
    public int Value { get; set; }
}

public sealed class MediHostOptions
{
    public int Value { get; set; }
}

public sealed class MediHostedService : IHostedService
{
    public bool Started { get; private set; }

    public bool Stopped { get; private set; }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Started = true;
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        Stopped = true;
        return Task.CompletedTask;
    }
}
