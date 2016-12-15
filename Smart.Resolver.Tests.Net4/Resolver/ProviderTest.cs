namespace Smart.Resolver
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Collections.Generic;
    using Smart.Resolver.Mocks;

    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class ProviderTest
    {
        [TestMethod]
        public void ObjectCreatedByConstantProvider()
        {
            var config = new ResolverConfig();
            var service = new Service();
            config.Bind<IService>().ToConstant(service);
            config.Bind<Controller>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var controller = resolver.Get<Controller>();

                Assert.AreSame(service, controller.Service);
            }
        }

        [TestMethod]
        public void ObjectCreatedByCallbackProvider()
        {
            var config = new ResolverConfig();
            var service = new Service();
            config.Bind<IService>().ToMethod(k => service);
            config.Bind<Controller>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var controller = resolver.Get<Controller>();

                Assert.AreSame(service, controller.Service);
            }
        }

        [TestMethod]
        public void ObjectArrayCreatedByStandardProvider()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            config.Bind<ArrayInjectedObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<ArrayInjectedObject>();
                var foo = resolver.Get<SimpleObject>("foo");
                var bar = resolver.Get<SimpleObject>("bar");

                Assert.AreEqual(2, obj.Objects.Length);
                Assert.AreNotSame(foo, bar);
                Assert.IsTrue(obj.Objects.Contains(foo, x => x));
                Assert.IsTrue(obj.Objects.Contains(bar, x => x));
            }
        }

        [TestMethod]
        public void ObjectEnumerableCreatedByStandardProvider()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            config.Bind<EnumerableInjectedObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<EnumerableInjectedObject>();
                var foo = resolver.Get<SimpleObject>("foo");
                var bar = resolver.Get<SimpleObject>("bar");

                Assert.AreEqual(2, obj.Objects.Count());
                Assert.AreNotSame(foo, bar);
                Assert.IsTrue(obj.Objects.Contains(foo, x => x));
                Assert.IsTrue(obj.Objects.Contains(bar, x => x));
            }
        }

        [TestMethod]
        public void ObjectCollectionCreatedByStandardProvider()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            config.Bind<CollectionInjectedObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<CollectionInjectedObject>();
                var foo = resolver.Get<SimpleObject>("foo");
                var bar = resolver.Get<SimpleObject>("bar");

                Assert.AreEqual(2, obj.Objects.Count);
                Assert.AreNotSame(foo, bar);
                Assert.IsTrue(obj.Objects.Contains(foo, x => x));
                Assert.IsTrue(obj.Objects.Contains(bar, x => x));
            }
        }

        [TestMethod]
        public void ObjectListCreatedByStandardProvider()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            config.Bind<ListInjectedObject>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<ListInjectedObject>();
                var foo = resolver.Get<SimpleObject>("foo");
                var bar = resolver.Get<SimpleObject>("bar");

                Assert.AreEqual(2, obj.Objects.Count);
                Assert.AreNotSame(foo, bar);
                Assert.IsTrue(obj.Objects.Contains(foo, x => x));
                Assert.IsTrue(obj.Objects.Contains(bar, x => x));
            }
        }

        [TestMethod]
        public void ObjectIsCreatedByMaxParameterConstructor()
        {
            var config = new ResolverConfig();
            config.Bind<IService>().To<Service>().InSingletonScope();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<MultiConstructorObject>();

                Assert.AreEqual(2, obj.Arguments);
            }
        }

        [TestMethod]
        public void ObjectIsCreatedByBestConstructor()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<MultiConstructorObject>();

                Assert.AreEqual(1, obj.Arguments);
            }
        }

        protected class MultiConstructorObject
        {
            public int Arguments { get; }

            public SimpleObject SimpleObject { get; }

            public IService Service { get; }

            public MultiConstructorObject(SimpleObject simpleObject)
            {
                Arguments = 1;
                SimpleObject = simpleObject;
            }

            public MultiConstructorObject(SimpleObject simpleObject, IService service)
            {
                Arguments = 2;
                SimpleObject = simpleObject;
                Service = service;
            }
        }

        protected class ArrayInjectedObject
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1819:PropertiesShouldNotReturnArrays", Justification = "Ignore")]
            public SimpleObject[] Objects { get; }

            public ArrayInjectedObject(SimpleObject[] objects)
            {
                Objects = objects;
            }
        }

        protected class EnumerableInjectedObject
        {
            public IEnumerable<SimpleObject> Objects { get; }

            public EnumerableInjectedObject(IEnumerable<SimpleObject> objects)
            {
                Objects = objects;
            }
        }

        protected class CollectionInjectedObject
        {
            public ICollection<SimpleObject> Objects { get; }

            public CollectionInjectedObject(ICollection<SimpleObject> objects)
            {
                Objects = objects;
            }
        }

        protected class ListInjectedObject
        {
            public IList<SimpleObject> Objects { get; }

            public ListInjectedObject(IList<SimpleObject> objects)
            {
                Objects = objects;
            }
        }
    }
}
