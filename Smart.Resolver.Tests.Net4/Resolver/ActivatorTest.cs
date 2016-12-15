namespace Smart.Resolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Activators;
    using Smart.Resolver.Mocks;

    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class ActivatorTest
    {
        [TestMethod]
        public void ObjectIsInitializedOnCreation()
        {
            var config = new ResolverConfig();
            config.UseActivator<InitializeActivator>();
            config.Bind<InitializableObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<InitializableObject>();

                Assert.AreEqual(1, obj.InitializedCount);
            }
        }

        [TestMethod]
        public void ObjectIsNotInitializedOnInjection()
        {
            var config = new ResolverConfig();
            using (var resolver = config.ToResolver())
            {
                var obj = new InitializableObject();
                resolver.Inject(obj);

                Assert.AreEqual(0, obj.InitializedCount);
            }
        }

        [TestMethod]
        public void ObjectIsInitializedAtOnceInSingletonScope()
        {
            var config = new ResolverConfig();
            config.UseActivator<InitializeActivator>();
            config.Bind<InitializableObject>().ToSelf().InSingletonScope();

            using (var resolver = config.ToResolver())
            {
                var obj1 = resolver.Get<InitializableObject>();
                var obj2 = resolver.Get<InitializableObject>();

                Assert.AreSame(obj1, obj2);
                Assert.AreEqual(1, obj2.InitializedCount);
            }
        }

        [TestMethod]
        public void ObjectIsNotInitializedWhenActivatorDisabled()
        {
            var config = new ResolverConfig();
            config.Bind<InitializableObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<InitializableObject>();

                Assert.AreEqual(0, obj.InitializedCount);
            }
        }

        [TestMethod]
        public void ObjectIsCustomInitializedWhenActivatorCustomized()
        {
            var config = new ResolverConfig();
            config.UseActivator<CustomInitializeActivator>();
            config.Bind<CustomInitializableObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<CustomInitializableObject>();

                Assert.AreEqual(true, obj.Initialized);
            }
        }

        protected interface ICustomInitializable
        {
            void Initialize();
        }

        protected class CustomInitializeActivator : IActivator
        {
            public void Activate(object instance)
            {
                (instance as ICustomInitializable)?.Initialize();
            }
        }

        protected class CustomInitializableObject : ICustomInitializable
        {
            public bool Initialized { get; private set; }

            public void Initialize()
            {
                Initialized = true;
            }
        }
    }
}
