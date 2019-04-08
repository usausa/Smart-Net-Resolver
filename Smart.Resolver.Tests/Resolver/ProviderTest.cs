namespace Smart.Resolver
{
    using System.Collections.Generic;
    using System.Linq;

    using Smart.Collections.Generic;
    using Smart.Resolver.Mocks;

    using Xunit;

    public class ProviderTest
    {
        [Fact]
        public void ObjectCreatedByConstantProvider()
        {
            var config = new ResolverConfig();
            var service = new Service();
            config.Bind<IService>().ToConstant(service);
            config.Bind<Controller>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var controller = resolver.Get<Controller>();

                Assert.Same(service, controller.Service);
            }
        }

        [Fact]
        public void ObjectCreatedByCallbackProvider()
        {
            var config = new ResolverConfig();
            var service = new Service();
            config.Bind<IService>().ToMethod(k => service);
            config.Bind<Controller>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var controller = resolver.Get<Controller>();

                Assert.Same(service, controller.Service);
            }
        }

        [Fact]
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

                Assert.Equal(2, obj.Objects.Length);
                Assert.NotSame(foo, bar);
                Assert.True(obj.Objects.Contains(foo, x => x));
                Assert.True(obj.Objects.Contains(bar, x => x));
            }
        }

        [Fact]
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

                Assert.Equal(2, obj.Objects.Count());
                Assert.NotSame(foo, bar);
                Assert.True(obj.Objects.Contains(foo, x => x));
                Assert.True(obj.Objects.Contains(bar, x => x));
            }
        }

        [Fact]
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

                Assert.Equal(2, obj.Objects.Count);
                Assert.NotSame(foo, bar);
                Assert.True(obj.Objects.Contains(foo, x => x));
                Assert.True(obj.Objects.Contains(bar, x => x));
            }
        }

        [Fact]
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

                Assert.Equal(2, obj.Objects.Count);
                Assert.NotSame(foo, bar);
                Assert.True(obj.Objects.Contains(foo, x => x));
                Assert.True(obj.Objects.Contains(bar, x => x));
            }
        }

        [Fact]
        public void ObjectIsCreatedByMaxParameterConstructor()
        {
            var config = new ResolverConfig().UseAutoBinding();
            config.Bind<IService>().To<Service>().InSingletonScope();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<MultiConstructorObject>();

                Assert.Equal(2, obj.Arguments);
            }
        }

        [Fact]
        public void ObjectIsCreatedByBestConstructor()
        {
            var config = new ResolverConfig().UseAutoBinding();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<MultiConstructorObject>();

                Assert.Equal(1, obj.Arguments);
            }
        }

        public class MultiConstructorObject
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

        public class ArrayInjectedObject
        {
            public SimpleObject[] Objects { get; }

            public ArrayInjectedObject(SimpleObject[] objects)
            {
                Objects = objects;
            }
        }

        public class EnumerableInjectedObject
        {
            public IEnumerable<SimpleObject> Objects { get; }

            public EnumerableInjectedObject(IEnumerable<SimpleObject> objects)
            {
                Objects = objects;
            }
        }

        public class CollectionInjectedObject
        {
            public ICollection<SimpleObject> Objects { get; }

            public CollectionInjectedObject(ICollection<SimpleObject> objects)
            {
                Objects = objects;
            }
        }

        public class ListInjectedObject
        {
            public IList<SimpleObject> Objects { get; }

            public ListInjectedObject(IList<SimpleObject> objects)
            {
                Objects = objects;
            }
        }
    }
}
