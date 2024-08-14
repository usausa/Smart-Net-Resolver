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
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
        config.Bind<NameConstraintInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<NameConstraintInjectedObject>();
        var foo = resolver.Get<SimpleObject>("foo");
        var bar = resolver.Get<SimpleObject>("bar");

        Assert.Same(obj.SimpleObject, foo);
        Assert.NotSame(obj.SimpleObject, bar);
    }

    [Fact]
    public void ObjectIsSelectedByHasMetadataConstraint()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().WithMetadata("hoge", null);
        config.Bind<HasMetadataConstraintInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<HasMetadataConstraintInjectedObject>();
        var hoge = resolver.Get(typeof(SimpleObject), new HasMetadataConstraint("hoge"));

        Assert.Same(obj.SimpleObject, hoge);
    }

    [Fact]
    public void ObjectIsSelectedByChainConstraint()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope();
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().WithMetadata("hoge", null);
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
        config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar").WithMetadata("hoge", null);
        config.Bind<ChainConstraintInjectedObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<ChainConstraintInjectedObject>();
        var barHoge = resolver.Get(typeof(SimpleObject), new ChainConstraint(new KeyConstraint("bar"), new HasMetadataConstraint("hoge")));

        Assert.Same(obj.SimpleObject, barHoge);
    }

    public sealed class HasMetadataConstraint : IConstraint
    {
        public string Key { get; }

        public HasMetadataConstraint(string key)
        {
            Key = key;
        }

        public bool Match(BindingMetadata metadata)
        {
            return metadata.Has(Key);
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]
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

    public sealed class NameConstraintInjectedObject
    {
        public SimpleObject SimpleObject { get; }

        public NameConstraintInjectedObject([Keyed("foo")] SimpleObject simpleObject)
        {
            SimpleObject = simpleObject;
        }
    }

    public sealed class HasMetadataConstraintInjectedObject
    {
        public SimpleObject SimpleObject { get; }

        public HasMetadataConstraintInjectedObject([HasMetadata("hoge")] SimpleObject simpleObject)
        {
            SimpleObject = simpleObject;
        }
    }

    public sealed class ChainConstraintInjectedObject
    {
        public SimpleObject SimpleObject { get; }

        public ChainConstraintInjectedObject([Keyed("bar")][HasMetadata("hoge")] SimpleObject simpleObject)
        {
            SimpleObject = simpleObject;
        }
    }
}
