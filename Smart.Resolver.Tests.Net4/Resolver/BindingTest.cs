namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Handlers;
    using Smart.Resolver.Mocks;
    using Smart.Resolver.Providers;

    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class BindingTest
    {
        [TestMethod]
        public void ObjectBindingCreatedBySelfBindingResolver()
        {
            var config = new ResolverConfig();
            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<SimpleObject>();

                Assert.IsNotNull(obj);
            }
        }

        [TestMethod]
        public void ObjectBindingCreatedByOpenGenericBindingResolver()
        {
            var config = new ResolverConfig();
            config
                .Bind(typeof(IGenericService<>))
                .To(typeof(GenericService<>));

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get(typeof(IGenericService<int>));

                Assert.IsNotNull(obj);
                Assert.AreEqual(obj.GetType(), typeof(GenericService<int>));
            }
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
            var config = new ResolverConfig();
            var typeMap = new Dictionary<Type, Type>
            {
                [typeof(IService)] = typeof(Service)
            };
            config.UseMissingHandler(new CustomMissingHandler(typeMap));

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<IService>();

                Assert.IsNotNull(obj);
            }
        }

        protected class CustomMissingHandler : IMissingHandler
        {
            private readonly Dictionary<Type, Type> typeMap;

            public CustomMissingHandler(Dictionary<Type, Type> typeMap)
            {
                this.typeMap = typeMap;
            }

            public IEnumerable<IBinding> Handle(IComponentContainer components, IBindingTable table, Type type)
            {
                Type targetType;
                if (!typeMap.TryGetValue(type, out targetType))
                {
                    return Enumerable.Empty<IBinding>();
                }

                return new[]
                {
                    new Binding(type, new StandardProvider(typeof(Service), components))
                };
            }
        }
    }
}
