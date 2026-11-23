namespace Smart.Resolver;

using System.Runtime.InteropServices;

using Smart.Resolver.Builders;
using Smart.Resolver.Providers;
using Smart.Resolver.Scopes;

public sealed class EmitFactoryBuilderTests
{
    [Fact]
    public void ConstantIsPassedToInterfaceParameter()
    {
        var constant = new ValueObject();
        var factory = new EmitFactoryBuilder().CreateFactory(typeof(ValueConsumer).GetConstructors().Single(), [_ => constant], [constant], []);

        var obj = (ValueConsumer)factory(null!);

        Assert.Same(constant, obj.Value);
    }

    [Fact]
    public void ConstructorCanCallConstantDependencies()
    {
        var first = new ValueObject();
        var second = new DerivedValueObject();
        var factory = new EmitFactoryBuilder().CreateFactory(typeof(SumConsumer).GetConstructors().Single(), [_ => first, _ => second], [first, second], []);

        var obj = (SumConsumer)factory(null!);

        Assert.Equal(3, obj.Sum);
    }

    [Fact]
    public void TypedFactoryIsPassedToInterfaceParameter()
    {
        var factory = new EmitFactoryBuilder().CreateFactory(typeof(ValueConsumer).GetConstructors().Single(), [Typed(static _ => new ValueObject())], [null], []);

        var obj = (ValueConsumer)factory(null!);

        Assert.IsType<ValueObject>(obj.Value);
    }

    [Fact]
    public void FactoryReturningDerivedTypeIsPassedToInterfaceParameter()
    {
        var factory = new EmitFactoryBuilder().CreateFactory(typeof(ValueConsumer).GetConstructors().Single(), [Typed<ValueObject>(static _ => new DerivedValueObject())], [null], []);

        var obj = (ValueConsumer)factory(null!);

        Assert.IsType<DerivedValueObject>(obj.Value);
    }

    [Fact]
    public void ConstantAndTypedFactoryArePassedToInterfaceParameters()
    {
        var constant = new ValueObject();
        var factory = new EmitFactoryBuilder().CreateFactory(typeof(SumConsumer).GetConstructors().Single(), [_ => constant, Typed(static _ => new DerivedValueObject())], [constant, null], []);

        var obj = (SumConsumer)factory(null!);

        Assert.Same(constant, obj.First);
        Assert.IsType<DerivedValueObject>(obj.Second);
        Assert.Equal(3, obj.Sum);
    }

    [Fact]
    public void TypedFactoriesAreStoredInInterfaceArray()
    {
        var factory = new EmitFactoryBuilder().CreateArrayFactory(
            typeof(IValue),
            [Typed(static _ => new ValueObject()), Typed<ValueObject>(static _ => new DerivedValueObject()), Typed<object>(static _ => new ValueObject())]);

        var array = (IValue[])factory(null!);

        Assert.Equal([typeof(ValueObject), typeof(DerivedValueObject), typeof(ValueObject)], array.Select(static x => x.GetType()));
    }

    [Fact]
    public void ExpandedAndTypedFactoriesAreStoredInInterfaceArray()
    {
        var builder = new EmitFactoryBuilder();
        var expanded = builder.CreateFactory(typeof(ValueObject).GetConstructors().Single(), [], [], []);
        var factory = builder.CreateArrayFactory(typeof(IValue), [expanded, Typed<ValueObject>(static _ => new DerivedValueObject())]);

        var array = (IValue[])factory(null!);

        Assert.Equal([typeof(ValueObject), typeof(DerivedValueObject)], array.Select(static x => x.GetType()));
    }

    [Fact]
    public void ConstantOfUnrelatedTypeThrowsInvalidCastException()
    {
        var constant = new UnrelatedObject();
        var factory = new EmitFactoryBuilder().CreateFactory(typeof(ValueConsumer).GetConstructors().Single(), [_ => constant], [constant], []);

        Assert.Throws<InvalidCastException>(() => factory(null!));
    }

    [Fact]
    public void DynamicInterfaceCastableConstantIsPassedToInterfaceParameter()
    {
        var constant = new DynamicValueObject();
        var factory = new EmitFactoryBuilder().CreateFactory(typeof(ValueConsumer).GetConstructors().Single(), [_ => constant], [constant], []);

        var obj = (ValueConsumer)factory(null!);

        Assert.Same(constant, obj.Value);
        Assert.Equal(3, obj.Value.Value);
    }

    [Fact]
    public void ContainerScopedLeafIsPassedToInterfaceParameter()
    {
        var builder = new EmitFactoryBuilder();
        var scoped = new ContainerScope().Create(null!, builder.CreateFactory(typeof(ValueObject).GetConstructors().Single(), [], [], []));
        var factory = builder.CreateFactory(typeof(ValueConsumer).GetConstructors().Single(), [scoped], [null], []);

        var obj = (ValueConsumer)factory(null!);

        Assert.IsType<ValueObject>(obj.Value);
    }

    [Fact]
    public void ContainerScopedFactoriesAreStoredInInterfaceArray()
    {
        var builder = new EmitFactoryBuilder();
        var leaf = new ContainerScope().Create(null!, builder.CreateFactory(typeof(ValueObject).GetConstructors().Single(), [], [], []));
        var typed = new ContainerScope().Create(null!, Typed<ValueObject>(static _ => new DerivedValueObject()));
        var factory = builder.CreateArrayFactory(typeof(IValue), [leaf, typed]);

        var array = (IValue[])factory(null!);

        Assert.Equal([typeof(ValueObject), typeof(DerivedValueObject)], array.Select(static x => x.GetType()));
    }

    [Fact]
    public void ContainerScopedFactoryOfUnrelatedTypeThrowsInvalidCastException()
    {
        var scoped = new ContainerScope().Create(null!, Typed(static _ => new UnrelatedObject()));
        var factory = new EmitFactoryBuilder().CreateFactory(typeof(ValueConsumer).GetConstructors().Single(), [scoped], [null], []);

        Assert.Throws<InvalidCastException>(() => factory(null!));
    }

    [Fact]
    public void SingletonAndExpandedFactoryAreStoredInInterfaceArray()
    {
        using var resolver = new ResolverConfig().ToResolver();
        using var scope = new SingletonScope(resolver.Components);
        var builder = new EmitFactoryBuilder();
        var singleton = scope.Create(resolver, Typed(static _ => new ValueObject()));
        var expanded = builder.CreateFactory(typeof(ValueObject).GetConstructors().Single(), [], [], []);
        var factory = builder.CreateArrayFactory(typeof(IValue), [singleton, expanded]);

        var first = (IValue[])factory(null!);
        var second = (IValue[])factory(null!);

        Assert.Same(singleton(null!), first[0]);
        Assert.Same(first[0], second[0]);
        Assert.NotSame(first[1], second[1]);
    }

    [Fact]
    public void ConstantsAreStoredInInterfaceArray()
    {
        var constant1 = new ValueObject();
        var constant2 = new DerivedValueObject();
        var factory = new EmitFactoryBuilder().CreateArrayFactory(
            typeof(IValue),
            [new ConstantProvider<IValue>(constant1).CreateFactory(null!, null!, null), new ConstantProvider<IValue>(constant2).CreateFactory(null!, null!, null)]);

        var array = (IValue[])factory(null!);

        Assert.Same(constant1, array[0]);
        Assert.Same(constant2, array[1]);
    }

    [Fact]
    public void ConstantOfUnrelatedTypeInArrayThrowsArrayTypeMismatchException()
    {
        var factory = new EmitFactoryBuilder().CreateArrayFactory(typeof(IValue), [new ConstantProvider<object>(new UnrelatedObject()).CreateFactory(null!, null!, null)]);

        Assert.Throws<ArrayTypeMismatchException>(() => factory(null!));
    }

    private static Func<IResolver, T> Typed<T>(Func<IResolver, T> factory) => factory;

    public interface IValue
    {
        int Value { get; }
    }

    [DynamicInterfaceCastableImplementation]
    public interface IDynamicValue : IValue
    {
        int IValue.Value => 3;
    }

    public class ValueObject : IValue
    {
        public virtual int Value => 1;
    }

    public sealed class DerivedValueObject : ValueObject
    {
        public override int Value => 2;
    }

    public sealed class DynamicValueObject : IDynamicInterfaceCastable
    {
        public bool IsInterfaceImplemented(RuntimeTypeHandle interfaceType, bool throwIfNotImplemented) => interfaceType.Equals(typeof(IValue).TypeHandle);

        public RuntimeTypeHandle GetInterfaceImplementation(RuntimeTypeHandle interfaceType) => typeof(IDynamicValue).TypeHandle;
    }

    public sealed class UnrelatedObject;

    public sealed class ValueConsumer(IValue value)
    {
        public IValue Value { get; } = value;
    }

    public sealed class SumConsumer(IValue first, IValue second)
    {
        public IValue First { get; } = first;

        public IValue Second { get; } = second;

        public int Sum { get; } = first.Value + second.Value;
    }
}
