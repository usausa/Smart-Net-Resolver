namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Mocks;
    using Smart.Resolver.Providers;

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
        public void ObjectBindingCreatedBySelfBindingResolver()
        {
            var obj = resolver.Get<SimpleObject>();

            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void ObjectBindingCreatedByOpenGenericBindingResolver()
        {
            resolver
                .Bind(typeof(IGenericService<>))
                .To(typeof(GenericService<>));

            var obj = resolver.Get(typeof(IGenericService<int>));

            Assert.IsNotNull(obj);
            Assert.AreEqual(obj.GetType(), typeof(GenericService<int>));
        }

        protected interface IGenericService<out T>
        {
            T Create();
        }

        protected class GenericService<T> : IGenericService<T>
        {
            public T Create()
            {
                return default(T);
            }
        }

        [TestMethod]
        public void ObjectBindingCreatedByCustomBindingResolver()
        {
            var typeMap = new Dictionary<Type, Type>
            {
                [typeof(IService)] = typeof(Service)
            };
            resolver.Configure(c => c.Get<IMissingPipeline>().Resolvers.Add(new CustomBindingResolver(typeMap)));

            var obj = resolver.Get<IService>();

            Assert.IsNotNull(obj);
        }

        protected class CustomBindingResolver : IBindingResolver
        {
            private readonly Dictionary<Type, Type> typeMap;

            public CustomBindingResolver(Dictionary<Type, Type> typeMap)
            {
                this.typeMap = typeMap;
            }

            public IEnumerable<IBinding> Resolve(IResolverContext context, Type type)
            {
                Type targetType;
                if (!typeMap.TryGetValue(type, out targetType))
                {
                    return Enumerable.Empty<IBinding>();
                }

                return new[]
                {
                    new Binding(type, new BindingMetadata())
                    {
                        Provider = new StandardProvider(targetType)
                    }
                };
            }
        }
    }
}
