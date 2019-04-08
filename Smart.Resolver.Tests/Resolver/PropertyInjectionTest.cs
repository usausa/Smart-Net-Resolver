namespace Smart.Resolver
{
    using Smart.Resolver.Mocks;

    using Xunit;

    public class PropertyInjectionTest
    {
        [Fact]
        public void ObjectIsInjectedOnCreationWhenPropertyInjectorEnabled()
        {
            var config = new ResolverConfig();
            config.UsePropertyInjector();
            config.Bind<SimpleObject>().ToSelf();
            config.Bind<HasPropertyObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<HasPropertyObject>();

                Assert.NotNull(obj.Injected);
            }
        }

        [Fact]
        public void ObjectIsInjectedByInjectWhenPropertyInjectorEnabled()
        {
            var config = new ResolverConfig();
            config.UsePropertyInjector();
            config.Bind<SimpleObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = new HasPropertyObject();
                resolver.Inject(obj);

                Assert.NotNull(obj.Injected);
            }
        }

        [Fact]
        public void ObjectIsNotInjectedOnCreationWhenDefault()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf();
            config.Bind<HasPropertyObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<HasPropertyObject>();

                Assert.Null(obj.Injected);
            }
        }

        [Fact]
        public void ObjectIsNotInjectedByInjectWhenDefault()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = new HasPropertyObject();
                resolver.Inject(obj);

                Assert.Null(obj.Injected);
            }
        }
    }
}
