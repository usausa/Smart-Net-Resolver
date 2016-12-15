namespace Smart.Resolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Mocks;

    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class PropertyInjectionTest
    {
        [TestMethod]
        public void ObjectIsInjectedOnCreationWhenPropertyInjectorEnabled()
        {
            var config = new ResolverConfig();
            config.UsePropertyInjector();
            config.Bind<SimpleObject>().ToSelf();
            config.Bind<HasPropertyObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<HasPropertyObject>();

                Assert.IsNotNull(obj.Injected);
            }
        }

        [TestMethod]
        public void ObjectIsInjectedByInjectWhenPropertyInjectorEnabled()
        {
            var config = new ResolverConfig();
            config.UsePropertyInjector();
            config.Bind<SimpleObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = new HasPropertyObject();
                resolver.Inject(obj);

                Assert.IsNotNull(obj.Injected);
            }
        }

        [TestMethod]
        public void ObjectIsNotInjectedOnCreationWhenDefault()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf();
            config.Bind<HasPropertyObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<HasPropertyObject>();

                Assert.IsNull(obj.Injected);
            }
        }

        [TestMethod]
        public void ObjectIsNotInjectedByInjectWhenDefault()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = new HasPropertyObject();
                resolver.Inject(obj);

                Assert.IsNull(obj.Injected);
            }
        }
    }
}
