namespace Smart.Resolver
{
    using Smart.Resolver.Mocks;
    using Smart.Resolver.Processors;

    using Xunit;

    /// <summary>
    ///
    /// </summary>
    public class ProcessorTest
    {
        [Fact]
        public void ObjectIsInitializedOnCreation()
        {
            var config = new ResolverConfig();
            config.UseProcessor<InitializeProcessor>();
            config.Bind<InitializableObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<InitializableObject>();

                Assert.Equal(1, obj.InitializedCount);
            }
        }

        [Fact]
        public void ObjectIsNotInitializedOnInjection()
        {
            var config = new ResolverConfig();
            using (var resolver = config.ToResolver())
            {
                var obj = new InitializableObject();
                resolver.Inject(obj);

                Assert.Equal(0, obj.InitializedCount);
            }
        }

        [Fact]
        public void ObjectIsInitializedAtOnceInSingletonScope()
        {
            var config = new ResolverConfig();
            config.UseProcessor<InitializeProcessor>();
            config.Bind<InitializableObject>().ToSelf().InSingletonScope();

            using (var resolver = config.ToResolver())
            {
                var obj1 = resolver.Get<InitializableObject>();
                var obj2 = resolver.Get<InitializableObject>();

                Assert.Same(obj1, obj2);
                Assert.Equal(1, obj2.InitializedCount);
            }
        }

        [Fact]
        public void ObjectIsNotInitializedWhenProcessorDisabled()
        {
            var config = new ResolverConfig();
            config.Bind<InitializableObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<InitializableObject>();

                Assert.Equal(0, obj.InitializedCount);
            }
        }

        [Fact]
        public void ObjectIsCustomInitializedWhenProcessorCustomized()
        {
            var config = new ResolverConfig();
            config.UseProcessor<CustomInitializeProcessor>();
            config.Bind<CustomInitializableObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<CustomInitializableObject>();

                Assert.True(obj.Initialized);
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
