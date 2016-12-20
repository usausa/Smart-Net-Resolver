namespace Smart.Resolver
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Mocks;
    using Smart.Resolver.Processors;

    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class ProcessorTest
    {
        [TestMethod]
        public void ObjectIsInitializedOnCreation()
        {
            var config = new ResolverConfig();
            config.UseProcessor<InitializeProcessor>();
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
            config.UseProcessor<InitializeProcessor>();
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
        public void ObjectIsNotInitializedWhenProcessorDisabled()
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
        public void ObjectIsCustomInitializedWhenProcessorCustomized()
        {
            var config = new ResolverConfig();
            config.UseProcessor<CustomInitializeProcessor>();
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

        protected class CustomInitializeProcessor : IProcessor
        {
            public void Initialize(object instance)
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
