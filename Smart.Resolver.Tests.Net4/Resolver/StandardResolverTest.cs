namespace Smart.Resolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Mocks;

    /// <summary>
    /// ActivatorsTest の概要の説明
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Ignore")]
    [TestClass]
    public class StandardResolverTest
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
        public void ResolveSelf()
        {
            var obj = resolver.Get<IResolver>();

            Assert.AreSame(resolver, obj);
        }

        [TestMethod]
        public void UseCaseForWebControllerAndService()
        {
            resolver.Bind<IService>().To<Service>().InSingletonScope();
            resolver.Bind<Controller>().ToSelf();

            var controller1 = resolver.Get<Controller>();
            var controller2 = resolver.Get<Controller>();

            Assert.AreNotSame(controller1, controller2);
            Assert.AreSame(controller1.Service, controller2.Service);
        }
    }
}
