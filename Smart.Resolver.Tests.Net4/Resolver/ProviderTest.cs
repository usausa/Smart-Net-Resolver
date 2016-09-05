namespace Smart.Resolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Mocks;

    /// <summary>
    /// ActivatorsTest の概要の説明
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Ignore")]
    [TestClass]
    public class ProviderTest
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
        public void ObjectCreatedByConstantProvider()
        {
            var service = new Service();
            resolver.Bind<IService>().ToConstant(service);
            resolver.Bind<Controller>().ToSelf();

            var controller = resolver.Get<Controller>();

            Assert.AreSame(service, controller.Service);
        }

        [TestMethod]
        public void ObjectCreatedByCallbackProvider()
        {
            var service = new Service();
            resolver.Bind<IService>().ToMethod(_ => service);
            resolver.Bind<Controller>().ToSelf();

            var controller = resolver.Get<Controller>();

            Assert.AreSame(service, controller.Service);
        }
    }
}
