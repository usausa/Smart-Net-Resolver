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
        public void SelfResolved()
        {
            var obj = resolver.Get<IResolver>();

            Assert.AreSame(resolver, obj);
        }

        [TestMethod]
        public void SelfTryResolved()
        {
            bool result;
            var obj = resolver.TryGet<IResolver>(out result);

            Assert.IsTrue(result);
            Assert.AreSame(resolver, obj);
        }

        [TestMethod]
        public void SelfCanResolved()
        {
            var result = resolver.CanGet<IResolver>();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ObjectIsResolvedWhenBindIsExist()
        {
            resolver.Bind<SimpleObject>().ToSelf().InTransientScope();

            var obj = resolver.Get<SimpleObject>();

            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void ObjectIsResolvedWhenBindIsNotExist()
        {
            var obj = resolver.Get<SimpleObject>();

            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void ObjectIsTryResolvedWhenBindIsExist()
        {
            resolver.Bind<SimpleObject>().ToSelf().InTransientScope();

            bool result;
            var obj = resolver.TryGet<SimpleObject>(out result);

            Assert.IsTrue(result);
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void ObjectIsTryResolvedWhenBindIsNotExist()
        {
            bool result;
            var obj = resolver.TryGet<SimpleObject>(out result);

            Assert.IsTrue(result);
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void ObjectCanResolvedWhenBindIsExist()
        {
            resolver.Bind<SimpleObject>().ToSelf().InTransientScope();

            var result = resolver.CanGet<SimpleObject>();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ObjectCanResolvedWhenBindIsNotExist()
        {
            var result = resolver.CanGet<SimpleObject>();

            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ObjectIsNotResolvedWhenMultiBindIsExist()
        {
            resolver.Bind<SimpleObject>().ToSelf().InTransientScope();
            resolver.Bind<SimpleObject>().ToSelf().InTransientScope();

            resolver.Get<SimpleObject>();
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void ObjectIsNotTryResolvedWhenMultiBindIsExist()
        {
            resolver.Bind<SimpleObject>().ToSelf().InTransientScope();
            resolver.Bind<SimpleObject>().ToSelf().InTransientScope();

            bool result;
            resolver.TryGet<SimpleObject>(out result);
        }

        [TestMethod]
        public void ObjectCanResolvedWhenMultiBindIsExist()
        {
            resolver.Bind<SimpleObject>().ToSelf().InTransientScope();
            resolver.Bind<SimpleObject>().ToSelf().InTransientScope();

            var result = resolver.CanGet<SimpleObject>();

            Assert.IsTrue(result);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void InterfaceIsNotResolvedWhenBindIsNotExist()
        {
            resolver.Get<IService>();
        }

        [TestMethod]
        public void InterfaceIsNotTryResolvedWhenBindIsNotExist()
        {
            bool result;
            resolver.TryGet<IService>(out result);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void InterfaceIsNotCanResolvedWhenBindIsNotExist()
        {
            var result = resolver.CanGet<IService>();

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ObjectIsAllResolvedWhenMultiBinding()
        {
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");

            var objs = resolver.GetAll<SimpleObject>();

            foreach (var obj in objs)
            {
                Assert.AreEqual(obj.GetType(), typeof(SimpleObject));
            }
        }

        [TestMethod]
        public void UseCaseForWebControllerAndService()
        {
            resolver.Bind<IService>().To<Service>().InSingletonScope();
            resolver.Bind<Controller>().ToSelf();

            var controller1 = (Controller)resolver.Get(typeof(Controller));
            var controller2 = (Controller)resolver.Get(typeof(Controller));

            Assert.AreNotSame(controller1, controller2);
            Assert.AreSame(controller1.Service, controller2.Service);
        }
    }
}
