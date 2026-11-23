namespace Smart.Resolver;

using Smart.Resolver.Attributes;
using Smart.Resolver.Mocks;

public sealed class PropertyInjectionTests
{
    [Fact]
    public void ObjectIsInjectedOnCreationWhenPropertyInjectorEnabled()
    {
        var config = new ResolverConfig();
        config.UsePropertyInjector();
        config.Bind<SimpleObject>().ToSelf();
        config.Bind<HasPropertyObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<HasPropertyObject>();

        Assert.NotNull(obj.Injected);
    }

    [Fact]
    public void ObjectIsInjectedByInjectWhenPropertyInjectorEnabled()
    {
        var config = new ResolverConfig();
        config.UsePropertyInjector();
        config.Bind<SimpleObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = new HasPropertyObject();
        resolver.Inject(obj);

        Assert.NotNull(obj.Injected);
    }

    [Fact]
    public void ObjectIsInjectedToPrivateSetterPropertyWhenPropertyInjectorEnabled()
    {
        var config = new ResolverConfig();
        config.UsePropertyInjector();
        config.Bind<SimpleObject>().ToSelf();
        config.Bind<HasNonPublicPropertyObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<HasNonPublicPropertyObject>();

        Assert.NotNull(obj.PrivateSetter);
    }

    [Fact]
    public void ObjectIsInjectedToInternalPropertyWhenPropertyInjectorEnabled()
    {
        var config = new ResolverConfig();
        config.UsePropertyInjector();
        config.Bind<SimpleObject>().ToSelf();
        config.Bind<HasNonPublicPropertyObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<HasNonPublicPropertyObject>();

        Assert.NotNull(obj.Internal);
    }

    [Fact]
    public void ObjectIsInjectedToPrivatePropertyWhenPropertyInjectorEnabled()
    {
        var config = new ResolverConfig();
        config.UsePropertyInjector();
        config.Bind<SimpleObject>().ToSelf();
        config.Bind<HasNonPublicPropertyObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<HasNonPublicPropertyObject>();

        Assert.NotNull(obj.PrivateValue);
    }

    [Fact]
    public void ObjectIsInjectedToProtectedPropertyOfBaseClassWhenPropertyInjectorEnabled()
    {
        var config = new ResolverConfig();
        config.UsePropertyInjector();
        config.Bind<SimpleObject>().ToSelf();
        config.Bind<HasNonPublicPropertyObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<HasNonPublicPropertyObject>();

        Assert.NotNull(obj.ProtectedValue);
    }

    [Fact]
    public void ObjectIsNotInjectedOnCreationWhenDefault()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf();
        config.Bind<HasPropertyObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<HasPropertyObject>();

        Assert.Null(obj.Injected);
    }

    [Fact]
    public void ObjectIsNotInjectedByInjectWhenDefault()
    {
        var config = new ResolverConfig();
        config.Bind<SimpleObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = new HasPropertyObject();
        resolver.Inject(obj);

        Assert.Null(obj.Injected);
    }

    [Fact]
    public void ObjectIsInjectedWithPropertiesOfTypeArgument()
    {
        var config = new ResolverConfig();
        config.UsePropertyInjector();
        config.Bind<SimpleObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = new DerivedPropertyObject();
        BasePropertyObject target = obj;
        resolver.Inject(target);

        Assert.NotNull(obj.BaseInjected);
        Assert.Null(obj.DerivedInjected);
    }

    [Fact]
    public void ObjectIsInjectedWithPropertiesOfRuntimeTypeWhenPassedAsObject()
    {
        var config = new ResolverConfig();
        config.UsePropertyInjector();
        config.Bind<SimpleObject>().ToSelf();

        using var resolver = config.ToResolver();
        var obj = new DerivedPropertyObject();
        object target = obj;
        resolver.Inject(target);

        Assert.NotNull(obj.BaseInjected);
        Assert.NotNull(obj.DerivedInjected);
    }

    [Fact]
    public void ObjectIsInjectedByInjectOfChildResolver()
    {
        var config = new ResolverConfig();
        config.UsePropertyInjector();
        config.Bind<SimpleObject>().ToSelf();

        using var resolver = config.ToResolver();
        using var child = resolver.CreateChildResolver();
        var obj = new HasPropertyObject();
        child.Inject(obj);

        Assert.NotNull(obj.Injected);
    }

    public class BasePropertyObject
    {
        [Inject]
        public SimpleObject? BaseInjected { get; set; }
    }

    public sealed class DerivedPropertyObject : BasePropertyObject
    {
        [Inject]
        public SimpleObject? DerivedInjected { get; set; }
    }
}
