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
}
