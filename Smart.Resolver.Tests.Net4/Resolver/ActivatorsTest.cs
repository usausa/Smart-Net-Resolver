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
    public class ActivatorsTest
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
        public void ObjectIsInitializedOnCreation()
        {
            resolver.Bind<InitializableObject>().ToSelf();

            var obj = resolver.Get<InitializableObject>();

            Assert.AreEqual(1, obj.InitializedCount);
        }

        [TestMethod]
        public void ObjectIsNotInitializedOnInjection()
        {
            var obj = new InitializableObject();
            resolver.Inject(obj);

            Assert.AreEqual(0, obj.InitializedCount);
        }

        [TestMethod]
        public void ObjectIsInitializedAtOnceInSingletonScope()
        {
            resolver.Bind<InitializableObject>().ToSelf().InSingletonScope();

            var obj1 = resolver.Get<InitializableObject>();
            var obj2 = resolver.Get<InitializableObject>();

            Assert.AreSame(obj1, obj2);
            Assert.AreEqual(1, obj2.InitializedCount);
        }

        [TestMethod]
        public void ObjectIsNotInitializedWhenActivatorDisabled()
        {
            resolver.Configure(c => c.Get<IActivatePipeline>().Activators.Clear());
            resolver.Bind<InitializableObject>().ToSelf();

            var obj = resolver.Get<InitializableObject>();

            Assert.AreEqual(0, obj.InitializedCount);
        }

        [TestMethod]
        public void ObjectIsCustomInitializedWhenActivatorCustomized()
        {
            resolver.Configure(c => c.Get<IActivatePipeline>().Activators.Add(new CustomInitializeActivator()));
            resolver.Bind<CustomInitializableObject>().ToSelf();

            var obj = resolver.Get<CustomInitializableObject>();

            Assert.AreEqual(true, obj.Initialized);
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
