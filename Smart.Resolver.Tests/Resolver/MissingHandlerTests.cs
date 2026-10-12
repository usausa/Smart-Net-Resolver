namespace Smart.Resolver;

using Smart.Resolver.Mocks;

public sealed class MissingHandlerTests
{
    [Fact]
    public void StringIsNotCreatedByAutoBinding()
    {
        var config = new ResolverConfig().UseAutoBinding();

        using var resolver = config.ToResolver();

        Assert.False(resolver.TryGet<string>(out _));
    }

    [Fact]
    public void DelegateIsNotCreatedByAutoBinding()
    {
        var config = new ResolverConfig().UseAutoBinding();

        using var resolver = config.ToResolver();

        Assert.False(resolver.TryGet<Action<int>>(out _));
    }

    [Fact]
    public void ObjectIsNotResolvedByAssignableBinding()
    {
        var config = new ResolverConfig().UseAssignableBinding();
        config.Bind<SimpleObject>().ToSelf();

        using var resolver = config.ToResolver();

        Assert.False(resolver.TryGet<object>(out _));
    }

    [Fact]
    public void ArrayIsEmptyWhenAutoBoundTypeCannotBeCreated()
    {
        var config = new ResolverConfig().UseAutoBinding().UseArrayBinding();

        using var resolver = config.ToResolver();
        var registrations = resolver.Get<IEnumerable<Registration>>();

        Assert.Empty(registrations);
    }

    [Fact]
    public void InitializerIsResolvedWithoutRegistrations()
    {
        var config = new ResolverConfig().UseAutoBinding().UseArrayBinding().UseAssignableBinding();
        config.Bind<IInitializer>().To<RegistrationInitializer>().InSingletonScope();

        using var resolver = config.ToResolver();
        var initializers = resolver.Get<IEnumerable<IInitializer>>().ToArray();

        var initializer = Assert.IsType<RegistrationInitializer>(Assert.Single(initializers));
        Assert.Empty(initializer.Registrations);
    }

    public interface IInitializer
    {
        void Initialize();
    }

    public sealed class Registration
    {
        public Action<object> Configure { get; }

        public Registration(Action<object> configure)
        {
            Configure = configure;
        }
    }

    public sealed class RegistrationInitializer : IInitializer
    {
        public IEnumerable<Registration> Registrations { get; }

        public RegistrationInitializer(IEnumerable<Registration> registrations)
        {
            Registrations = registrations;
        }

        public void Initialize()
        {
        }
    }

    [Fact]
    public void CircularDependencyThrowsException()
    {
        var config = new ResolverConfig().UseAutoBinding();

        using var resolver = config.ToResolver();

        Assert.Throws<InvalidOperationException>(resolver.Get<CircularService1>);
    }

    public sealed class CircularService1
    {
        public CircularService2 Service { get; }

        public CircularService1(CircularService2 service)
        {
            Service = service;
        }
    }

    public sealed class CircularService2
    {
        public CircularService1 Service { get; }

        public CircularService2(CircularService1 service)
        {
            Service = service;
        }
    }

    [Fact]
    public void CanGetIsFalseWhenAutoBoundTypeCannotBeCreated()
    {
        var config = new ResolverConfig().UseAutoBinding();

        using var resolver = config.ToResolver();

        Assert.False(resolver.CanGet<Controller>());
        Assert.False(resolver.TryGet<Controller>(out _));
        Assert.False(resolver.CanGet<Controller>());
    }

    [Fact]
    public void TypeWithoutPublicConstructorIsNotCreatedByAutoBinding()
    {
        var config = new ResolverConfig().UseAutoBinding();

        using var resolver = config.ToResolver();

        Assert.False(resolver.CanGet<NoPublicConstructorObject>());
        Assert.False(resolver.TryGet<NoPublicConstructorObject>(out _));
    }

    [Fact]
    public void OtherConstructorIsUsedWhenAutoBoundArgumentCannotBeCreated()
    {
        var config = new ResolverConfig().UseAutoBinding();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<AlternativeConstructorObject>();

        Assert.Null(obj.Dependency);
    }

    [Fact]
    public void BoundTypeWithoutPublicConstructorThrowsException()
    {
        var config = new ResolverConfig();
        config.Bind<NoPublicConstructorObject>().ToSelf();

        using var resolver = config.ToResolver();

        Assert.Throws<InvalidOperationException>(resolver.Get<NoPublicConstructorObject>);
    }

    // ReSharper disable once ClassCannotBeInstantiated
    public sealed class NoPublicConstructorObject
    {
        private NoPublicConstructorObject()
        {
        }
    }

    public sealed class AlternativeConstructorObject
    {
        public NoPublicConstructorObject? Dependency { get; }

        public AlternativeConstructorObject(NoPublicConstructorObject dependency)
        {
            Dependency = dependency;
        }

        public AlternativeConstructorObject()
        {
        }
    }
}
