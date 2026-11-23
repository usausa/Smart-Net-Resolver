namespace Smart.Resolver;

using System.Reflection;

using Smart.Resolver.Attributes;
using Smart.Resolver.Builders;
using Smart.Resolver.Mocks;
using Smart.Resolver.Processors;

public sealed class ReflectionFactoryBuilderTests
{
    private static readonly Type[] Leaves = [typeof(Leaf1), typeof(Leaf2), typeof(Leaf3), typeof(Leaf4), typeof(Leaf5)];

    private static ResolverConfig CreateConfig()
    {
        var config = new ResolverConfig();
        config.Components.RemoveAll<IFactoryBuilder>();
        config.Components.Add<IFactoryBuilder, ReflectionFactoryBuilder>();
        config.Bind<Leaf1>().ToSelf();
        config.Bind<Leaf2>().ToSelf();
        config.Bind<Leaf3>().ToSelf();
        config.Bind<Leaf4>().ToSelf();
        config.Bind<Leaf5>().ToSelf();
        return config;
    }

    private static IReadOnlyList<object> ResolveArguments<T>()
        where T : class, IArityComponent
    {
        var config = CreateConfig();
        config.Bind<T>().ToSelf();

        using var resolver = config.ToResolver();
        return resolver.Get<T>().Arguments;
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(2)]
    [InlineData(3)]
    [InlineData(4)]
    [InlineData(5)]
    public void ResolvesEachConstructorArity(int arity)
    {
        var arguments = arity switch
        {
            0 => ResolveArguments<Arity0>(),
            1 => ResolveArguments<Arity1>(),
            2 => ResolveArguments<Arity2>(),
            3 => ResolveArguments<Arity3>(),
            4 => ResolveArguments<Arity4>(),
            _ => ResolveArguments<Arity5>()
        };

        Assert.Equal(Leaves.Take(arity), arguments.Select(static x => x.GetType()));
    }

    [Fact]
    public void ResolvesWithConstantArgument()
    {
        var config = CreateConfig();
        var constant = new Leaf2();
        config.Bind<Arity2>().ToSelf().WithConstructorArgument("b", constant);

        using var resolver = config.ToResolver();
        var obj = resolver.Get<Arity2>();

        Assert.Same(constant, obj.Arguments[1]);
    }

    [Fact]
    public void ResolvesWithPropertyInjectionWithoutArgument()
    {
        var config = CreateConfig();
        config.UsePropertyInjector();
        config.Bind<SimpleObject>().ToSelf();
        config.Bind<HasPropertyObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<HasPropertyObject>();

        Assert.NotNull(obj.Injected);
    }

    [Fact]
    public void ResolvesWithPropertyInjectionWithArgument()
    {
        var config = CreateConfig();
        config.UsePropertyInjector();
        config.Bind<PropertyArity1>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<PropertyArity1>();

        Assert.NotNull(obj.Leaf);
        Assert.NotNull(obj.Injected);
    }

    [Fact]
    public void ResolvesWithProcessor()
    {
        var config = CreateConfig();
        config.UseProcessor<InitializeProcessor>();
        config.Bind<InitializableObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<InitializableObject>();

        Assert.Equal(1, obj.InitializedCount);
    }

    [Fact]
    public void ResolvesArrayArgument()
    {
        var config = CreateConfig();
        config.Bind<IService>().To<Service1>();
        config.Bind<IService>().To<Service2>();
        config.Bind<ArrayConsumer>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<ArrayConsumer>();

        Assert.Equal([typeof(Service1), typeof(Service2)], obj.Services.Select(static x => x.GetType()));
    }

    [Fact]
    public void ConstructorExceptionWithoutArgumentIsWrappedOnJit()
    {
        var config = CreateConfig();
        config.Bind<ThrowingArity0>().ToSelf();

        using var resolver = config.ToResolver();

        Assert.Throws<TargetInvocationException>(resolver.Get<ThrowingArity0>);
    }

    [Fact]
    public void ConstructorExceptionWithArgumentIsNotWrapped()
    {
        var config = CreateConfig();
        config.Bind<ThrowingArity1>().ToSelf();

        using var resolver = config.ToResolver();

        Assert.Throws<InvalidOperationException>(resolver.Get<ThrowingArity1>);
    }

    public interface IArityComponent
    {
        IReadOnlyList<object> Arguments { get; }
    }

    public sealed class Leaf1;

    public sealed class Leaf2;

    public sealed class Leaf3;

    public sealed class Leaf4;

    public sealed class Leaf5;

    public sealed class Arity0 : IArityComponent
    {
        public IReadOnlyList<object> Arguments { get; } = [];
    }

    public sealed class Arity1(Leaf1 a) : IArityComponent
    {
        public IReadOnlyList<object> Arguments { get; } = [a];
    }

    public sealed class Arity2(Leaf1 a, Leaf2 b) : IArityComponent
    {
        public IReadOnlyList<object> Arguments { get; } = [a, b];
    }

    public sealed class Arity3(Leaf1 a, Leaf2 b, Leaf3 c) : IArityComponent
    {
        public IReadOnlyList<object> Arguments { get; } = [a, b, c];
    }

    public sealed class Arity4(Leaf1 a, Leaf2 b, Leaf3 c, Leaf4 d) : IArityComponent
    {
        public IReadOnlyList<object> Arguments { get; } = [a, b, c, d];
    }

    public sealed class Arity5(Leaf1 a, Leaf2 b, Leaf3 c, Leaf4 d, Leaf5 e) : IArityComponent
    {
        public IReadOnlyList<object> Arguments { get; } = [a, b, c, d, e];
    }

    public sealed class PropertyArity1(Leaf1 leaf)
    {
        public Leaf1 Leaf { get; } = leaf;

        [Inject]
        public Leaf2? Injected { get; set; }
    }

    public sealed class ArrayConsumer(IService[] services)
    {
        public IReadOnlyList<IService> Services { get; } = services;
    }

    public sealed class ThrowingArity0
    {
        public ThrowingArity0() => throw new InvalidOperationException();
    }

    public sealed class ThrowingArity1
    {
        public ThrowingArity1(Leaf1 leaf) => throw new InvalidOperationException(leaf.ToString());
    }
}
