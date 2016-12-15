namespace Smart.Resolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Mocks;

    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class StandardResolverTest
    {
        [TestMethod]
        public void SelfResolved()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<IResolver>();

                Assert.AreSame(resolver, obj);
            }
        }

        [TestMethod]
        public void SelfTryResolved()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                bool result;
                var obj = resolver.TryGet<IResolver>(out result);

                Assert.IsTrue(result);
                Assert.AreSame(resolver, obj);
            }
        }

        [TestMethod]
        public void SelfCanResolved()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var result = resolver.CanGet<IResolver>();

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ObjectIsResolvedWhenBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<SimpleObject>();

                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public void ObjectIsResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<SimpleObject>();

                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public void ObjectIsTryResolvedWhenBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                bool result;
                var obj = resolver.TryGet<SimpleObject>(out result);

                Assert.IsTrue(result);
                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public void ObjectIsTryResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                bool result;
                var obj = resolver.TryGet<SimpleObject>(out result);

                Assert.IsTrue(result);
                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public void ObjectCanResolvedWhenBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                var result = resolver.CanGet<SimpleObject>();

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ObjectCanResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var result = resolver.CanGet<SimpleObject>();

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ObjectIsResolvedWhenMultiBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                resolver.Get<SimpleObject>();
            }
        }

        [TestMethod]
        public void ObjectIsTryResolvedWhenMultiBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                bool result;
                resolver.TryGet<SimpleObject>(out result);

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        public void ObjectCanResolvedWhenMultiBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                var result = resolver.CanGet<SimpleObject>();

                Assert.IsTrue(result);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void InterfaceIsNotResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                resolver.Get<IService>();
            }
        }

        [TestMethod]
        public void InterfaceIsNotTryResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                bool result;
                resolver.TryGet<IService>(out result);

                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public void InterfaceIsNotCanResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var result = resolver.CanGet<IService>();

                Assert.IsFalse(result);
            }
        }

        [TestMethod]
        public void ObjectIsAllResolvedWhenMultiBinding()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");

            using (var resolver = config.ToResolver())
            {
                var objs = resolver.GetAll<SimpleObject>();

                foreach (var obj in objs)
                {
                    Assert.AreEqual(obj.GetType(), typeof(SimpleObject));
                }
            }
        }

        [TestMethod]
        public void UseCaseForWebControllerAndService()
        {
            var config = new ResolverConfig();
            config.Bind<IService>().To<Service>().InSingletonScope();
            config.Bind<Controller>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var controller1 = (Controller)resolver.Get(typeof(Controller));
                var controller2 = (Controller)resolver.Get(typeof(Controller));

                Assert.AreNotSame(controller1, controller2);
                Assert.AreSame(controller1.Service, controller2.Service);
            }
        }
    }
}
