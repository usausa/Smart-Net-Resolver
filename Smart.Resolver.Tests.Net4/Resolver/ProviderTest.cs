namespace Smart.Resolver
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Collections.Generic;
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
            resolver.Bind<IService>().ToMethod(k => service);
            resolver.Bind<Controller>().ToSelf();

            var controller = resolver.Get<Controller>();

            Assert.AreSame(service, controller.Service);
        }

        [TestMethod]
        public void ObjectArrayCreatedByStandardProvider()
        {
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            resolver.Bind<ArrayInjectedObject>().ToSelf();

            var obj = resolver.Get<ArrayInjectedObject>();
            var foo = resolver.Get<SimpleObject>("foo");
            var bar = resolver.Get<SimpleObject>("bar");

            Assert.AreEqual(2, obj.Objects.Length);
            Assert.AreNotSame(foo, bar);
            Assert.IsTrue(obj.Objects.Contains(foo, x => x));
            Assert.IsTrue(obj.Objects.Contains(bar, x => x));
        }

        [TestMethod]
        public void ObjectEnumerableCreatedByStandardProvider()
        {
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            resolver.Bind<EnumerableInjectedObject>().ToSelf();

            var obj = resolver.Get<EnumerableInjectedObject>();
            var foo = resolver.Get<SimpleObject>("foo");
            var bar = resolver.Get<SimpleObject>("bar");

            Assert.AreEqual(2, obj.Objects.Count());
            Assert.AreNotSame(foo, bar);
            Assert.IsTrue(obj.Objects.Contains(foo, x => x));
            Assert.IsTrue(obj.Objects.Contains(bar, x => x));
        }

        [TestMethod]
        public void ObjectCollectionCreatedByStandardProvider()
        {
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            resolver.Bind<CollectionInjectedObject>().ToSelf();

            var obj = resolver.Get<CollectionInjectedObject>();
            var foo = resolver.Get<SimpleObject>("foo");
            var bar = resolver.Get<SimpleObject>("bar");

            Assert.AreEqual(2, obj.Objects.Count);
            Assert.AreNotSame(foo, bar);
            Assert.IsTrue(obj.Objects.Contains(foo, x => x));
            Assert.IsTrue(obj.Objects.Contains(bar, x => x));
        }

        [TestMethod]
        public void ObjectListCreatedByStandardProvider()
        {
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");
            resolver.Bind<ListInjectedObject>().ToSelf();

            var obj = resolver.Get<ListInjectedObject>();
            var foo = resolver.Get<SimpleObject>("foo");
            var bar = resolver.Get<SimpleObject>("bar");

            Assert.AreEqual(2, obj.Objects.Count);
            Assert.AreNotSame(foo, bar);
            Assert.IsTrue(obj.Objects.Contains(foo, x => x));
            Assert.IsTrue(obj.Objects.Contains(bar, x => x));
        }

        [TestMethod]
        public void ObjectIsCreatedByMaxParameterConstructor()
        {
            resolver.Bind<IService>().To<Service>().InSingletonScope();

            var obj = resolver.Get<MultiConstructorObject>();

            Assert.AreEqual(2, obj.Arguments);
        }

        [TestMethod]
        public void ObjectIsCreatedByBestConstructor()
        {
            var obj = resolver.Get<MultiConstructorObject>();

            Assert.AreEqual(1, obj.Arguments);
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
