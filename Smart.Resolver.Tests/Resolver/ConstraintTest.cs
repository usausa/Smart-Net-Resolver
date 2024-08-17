namespace Smart.Resolver;

using Smart.Resolver.Attributes;
using Smart.Resolver.Bindings;
using Smart.Resolver.Constraints;
using Smart.Resolver.Mocks;

public sealed class ConstraintTest
{
    [Fact]
    public void ObjectIsSelectedByNameConstraint()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("foo");
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("bar");
        config.Bind<KeyedConstraintInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<KeyedConstraintInjectedObject>();
        var foo = resolver.Get<SimpleObject>("foo");
        var bar = resolver.Get<SimpleObject>("bar");

        Assert.Same(obj.SimpleObject, foo);
        Assert.NotSame(obj.SimpleObject, bar);
    }

    public sealed class KeyedConstraintInjectedObject
    {
        public SimpleObject SimpleObject { get; }

        public KeyedConstraintInjectedObject([ResolveBy("foo")] SimpleObject simpleObject)
        {
            SimpleObject = simpleObject;
        }
    }

    [Fact]
    public void ObjectIsInjectedByNameConstraint()
    {
        var config = new ResolverConfig();
        config.UsePropertyInjector();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("foo");
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Keyed("bar");
        config.Bind<KeyedConstraintPropertyObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<KeyedConstraintPropertyObject>();
        var foo = resolver.Get<SimpleObject>("foo");
        var bar = resolver.Get<SimpleObject>("bar");

        Assert.Same(obj.Injected, foo);
        Assert.NotSame(obj.Injected, bar);
    }

    public sealed class KeyedConstraintPropertyObject
    {
        [Inject]
        [ResolveBy("foo")]
        public SimpleObject? Injected { get; set; }
    }

    [Fact]
    public void ObjectIsSelectedByHasMetadataConstraint()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Constraint(new HasMetadataConstraint()).WithMetadata("hoge", null);
        config.Bind<HasMetadataConstraintInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<HasMetadataConstraintInjectedObject>();
        var hoge = resolver.Get(typeof(SimpleObject), "hoge");

        Assert.Same(obj.SimpleObject, hoge);
    }

    public sealed class HasMetadataConstraint : IConstraint
    {
        public bool Match(BindingMetadata metadata, object? key) => key is string str && metadata.Has(str);
    }

    public sealed class HasMetadataConstraintInjectedObject
    {
        public SimpleObject SimpleObject { get; }

        public HasMetadataConstraintInjectedObject([ResolveBy("hoge")] SimpleObject simpleObject)
        {
            SimpleObject = simpleObject;
        }
    }
}
