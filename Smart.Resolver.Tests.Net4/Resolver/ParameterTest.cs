namespace Smart.Resolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Mocks;

    /// <summary>
    /// ActivatorsTest の概要の説明
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Ignore")]
    [TestClass]
    public class ParameterTest
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
        public void ObjectIsInjectedOnCreationWithConstantParameter()
        {
            var service = new Service();
            resolver.Bind<Controller>().ToSelf().WithConstructorArgument("service", service);

            var controller = resolver.Get<Controller>();

            Assert.AreSame(service, controller.Service);
        }

        [TestMethod]
        public void ObjectIsInjectedOnCreationWithCallbackParameter()
        {
            resolver.Bind<IService>().To<Service>().InSingletonScope();
            resolver.Bind<Controller>().ToSelf().WithConstructorArgument("service", _ => _.Get<IService>());

            var controller = resolver.Get<Controller>();
            var service = resolver.Get<IService>();

            Assert.AreSame(service, controller.Service);
        }

        [TestMethod]
        public void ObjectIsInjectedWhenInjectWithConstantParameterOnCreation()
        {
            var injected = new SimpleObject();
            resolver.Bind<HasPropertyObject>().ToSelf().WithPropertyValue("Injected", injected);

            var obj = resolver.Get<HasPropertyObject>();

            Assert.AreSame(injected, obj.Injected);
        }

        [TestMethod]
        public void ObjectIsInjectedWhenInjectWithCallbavkParameterOnCreation()
        {
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope();
            resolver.Bind<HasPropertyObject>().ToSelf().WithPropertyValue("Injected", _ => _.Get<SimpleObject>());

            var obj = resolver.Get<HasPropertyObject>();
            var injected = resolver.Get<SimpleObject>();

            Assert.AreSame(injected, obj.Injected);
        }
    }
}
