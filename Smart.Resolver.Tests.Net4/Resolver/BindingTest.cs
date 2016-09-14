namespace Smart.Resolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Activators;
    using Smart.Resolver.Mocks;

    /// <summary>
    /// ActivatorsTest の概要の説明
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Ignore")]
    [TestClass]
    public class BindingTest
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
        public void ObjectCreatedWhenBindingNotExist()
        {
            var obj = resolver.Get<SimpleObject>();

            Assert.IsNotNull(obj);
        }
    }
}
