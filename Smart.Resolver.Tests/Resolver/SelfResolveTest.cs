namespace Smart.Resolver;

public sealed class SelfResolveTest
{
    [Fact]
    public void ResolveResolverInterface()
    {
        var config = new ResolverConfig();
        config.Bind<ResolverInterfaceReferenceObject>().ToSelf().InSingletonScope();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<ResolverInterfaceReferenceObject>();

        Assert.Same(resolver, obj.Resolver);
    }

    [Fact]
    public void ResolveResolver()
    {
        var config = new ResolverConfig();
        config.Bind<ResolverReferenceObject>().ToSelf().InSingletonScope();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<ResolverReferenceObject>();

        Assert.Same(resolver, obj.Resolver);
    }

    [Fact]
    public void ResolveServiceProvider()
    {
        var config = new ResolverConfig();
        config.Bind<ResolverServiceProviderObject>().ToSelf().InSingletonScope();

        using var resolver = config.ToResolver();
        var obj = resolver.Get<ResolverServiceProviderObject>();

        Assert.Same(resolver, obj.Provider);
    }

    public sealed class ResolverInterfaceReferenceObject
    {
        public IResolver Resolver { get; }

        public ResolverInterfaceReferenceObject(IResolver resolver)
        {
            Resolver = resolver;
        }
    }

    public sealed class ResolverReferenceObject
    {
        public SmartResolver Resolver { get; }

        public ResolverReferenceObject(SmartResolver resolver)
        {
            Resolver = resolver;
        }
    }

    public sealed class ResolverServiceProviderObject
    {
        public IServiceProvider Provider { get; }

        public ResolverServiceProviderObject(IServiceProvider provider)
        {
            Provider = provider;
        }
    }
}
