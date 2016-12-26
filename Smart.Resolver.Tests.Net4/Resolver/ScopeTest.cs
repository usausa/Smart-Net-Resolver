namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Mocks;
    using Smart.Resolver.Scopes;

    /// <summary>
    ///
    /// </summary>
    [TestClass]
    public class ScopeTest
    {
        [TestMethod]
        public void ObjectInTransientScopeAreNotSame()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                var obj1 = resolver.Get<SimpleObject>();
                var obj2 = resolver.Get<SimpleObject>();

                Assert.AreNotSame(obj1, obj2);
            }
        }

        [TestMethod]
        public void ObjectInSingletonScopeAreSame()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope();

            using (var resolver = config.ToResolver())
            {
                var obj1 = resolver.Get<SimpleObject>();
                var obj2 = resolver.Get<SimpleObject>();

                Assert.AreSame(obj1, obj2);
            }
        }

        [TestMethod]
        public void ObjectInCustomScope()
        {
            var config = new ResolverConfig();
            config.Components.Add<CustomScopeStorage>();
            config.Bind<SimpleObject>().ToSelf().InScope(c => new CustomScope(c));

            using (var resolver = config.ToResolver())
            {
                var obj1 = resolver.Get<SimpleObject>();
                var obj2 = resolver.Get<SimpleObject>();

                Assert.AreSame(obj1, obj2);
            }
        }

        [TestMethod]
        public void ObjectInSingletonScopeAreDisposedWhenResolverDisposed()
        {
            var config = new ResolverConfig();
            config.Bind<DisposeTrackingObject>().ToSelf().InSingletonScope();

            DisposeTrackingObject obj;
            using (var resolver = config.ToResolver())
            {
                obj = resolver.Get<DisposeTrackingObject>();
            }

            Assert.IsTrue(obj.Disposed);
        }

        protected sealed class DisposeTrackingObject : IDisposable
        {
            public bool Disposed { get; private set; }

            public void Dispose()
            {
                Disposed = true;
            }
        }

        protected class CustomScopeStorage : IScopeStorage
        {
            private static readonly ThreadLocal<Dictionary<IBinding, object>> Cache =
                new ThreadLocal<Dictionary<IBinding, object>>(() => new Dictionary<IBinding, object>());

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
            public object GetOrAdd(IBinding binding, Func<IBinding, object> factory)
            {
                object value;
                if (Cache.Value.TryGetValue(binding, out value))
                {
                    return value;
                }

                value = factory(binding);
                Cache.Value[binding] = value;

                return value;
            }

            public void Clear()
            {
                Cache.Value.Clear();
            }
        }

        protected class CustomScope : IScope
        {
            private readonly CustomScopeStorage storage;

            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
            public CustomScope(IComponentContainer components)
            {
                storage = components.Get<CustomScopeStorage>();
            }

            public IScopeStorage GetStorage(IKernel kernel)
            {
                return storage;
            }
        }
    }
}
