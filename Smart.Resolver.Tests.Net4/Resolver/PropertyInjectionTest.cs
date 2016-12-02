namespace Smart.Resolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Injectors;
    using Smart.Resolver.Mocks;

    /// <summary>
    /// ActivatorsTest の概要の説明
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Ignore")]
    [TestClass]
    public class PropertyInjectionTest
    {
        private StandardResolver resolver;

        [TestInitialize]
        public void TestInitialize()
        {
            resolver = new StandardResolver();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            resolver.Dispose();
        }

        [TestMethod]
        public void ObjectIsInjectedOnCreation()
        {
            resolver.Bind<SimpleObject>().ToSelf();
            resolver.Bind<HasPropertyObject>().ToSelf();

            var obj = resolver.Get<HasPropertyObject>();

            Assert.IsNotNull(obj.Injected);
        }

        [TestMethod]
        public void ObjectIsInjectedByInject()
        {
            resolver.Bind<SimpleObject>().ToSelf();

            var obj = new HasPropertyObject();
            resolver.Inject(obj);

            Assert.IsNotNull(obj.Injected);
        }

        [TestMethod]
        public void ObjectIsNotInjectedOnCreationWhenInjectorDisabled()
        {
            resolver.Configure(c => c.Remove<IInjectPipeline>());
            resolver.Bind<SimpleObject>().ToSelf();
            resolver.Bind<HasPropertyObject>().ToSelf();

            var obj = resolver.Get<HasPropertyObject>();

            Assert.IsNull(obj.Injected);
        }

        [TestMethod]
        public void ObjectIsNotInjectedByInjectWhenInjectorDisabled()
        {
            resolver.Configure(c => c.Remove<IInjectPipeline>());
            resolver.Bind<SimpleObject>().ToSelf();

            var obj = new HasPropertyObject();
            resolver.Inject(obj);

            Assert.IsNull(obj.Injected);
        }
    }
}
