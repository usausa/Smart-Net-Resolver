namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Mocks;
    using Smart.Resolver.Scopes;

    /// <summary>
    /// ActivatorsTest の概要の説明
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1001:TypesThatOwnDisposableFieldsShouldBeDisposable", Justification = "Ignore")]
    [TestClass]
    public class ScopeTest
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
        public void ObjectInTransientScopeAreNotSame()
        {
            resolver.Bind<SimpleObject>().ToSelf().InTransientScope();

            var obj1 = resolver.Get<SimpleObject>();
            var obj2 = resolver.Get<SimpleObject>();

            Assert.AreNotSame(obj1, obj2);
        }

        [TestMethod]
        public void ObjectInSingletonScopeAreSame()
        {
            resolver.Bind<SimpleObject>().ToSelf().InSingletonScope();

            var obj1 = resolver.Get<SimpleObject>();
            var obj2 = resolver.Get<SimpleObject>();

            Assert.AreSame(obj1, obj2);
        }

        [TestMethod]
        public void ObjectInCustomScope()
        {
            resolver.Configure(_ => _.Register(new CustomScopeStorage()));
            resolver.Bind<SimpleObject>().ToSelf().InScope(new CustomScope());

            var obj1 = resolver.Get<SimpleObject>();
            var obj2 = resolver.Get<SimpleObject>();

            Assert.AreSame(obj1, obj2);
        }

        [TestMethod]
        public void ObjectInSingletonScopeAreDisposedWhenResolverDisposed()
        {
            resolver.Bind<DisposeTrackingObject>().ToSelf().InSingletonScope();

            var obj = resolver.Get<DisposeTrackingObject>();

            resolver.Dispose();

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

            public void Remember(IBinding binding, object instance)
            {
                Cache.Value[binding] = instance;
            }

            public object TryGet(IBinding binding)
            {
                object value;
                return Cache.Value.TryGetValue(binding, out value) ? value : null;
            }

            public void Clear()
            {
                Cache.Value.Clear();
            }
        }

        protected class CustomScope : IScope
        {
            [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Ignore")]
            public IScopeStorage GetStorage(IKernel kernel)
            {
                return kernel.Components.Get<CustomScopeStorage>();
            }
        }
    }
}
