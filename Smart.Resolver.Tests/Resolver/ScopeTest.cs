namespace Smart.Resolver
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Factories;
    using Smart.Resolver.Mocks;
    using Smart.Resolver.Scopes;

    using Xunit;

    /// <summary>
    ///
    /// </summary>
    public class ScopeTest
    {
        [Fact]
        public void ObjectInTransientScopeAreNotSame()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                var obj1 = resolver.Get<SimpleObject>();
                var obj2 = resolver.Get<SimpleObject>();

                Assert.NotSame(obj1, obj2);
            }
        }

        [Fact]
        public void ObjectInSingletonScopeAreSame()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope();

            using (var resolver = config.ToResolver())
            {
                var obj1 = resolver.Get<SimpleObject>();
                var obj2 = resolver.Get<SimpleObject>();

                Assert.Same(obj1, obj2);
            }
        }

        [Fact]
        public void ObjectInCustomScope()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InScope(c => new CustomScope());

            using (var resolver = config.ToResolver())
            {
                var obj1 = resolver.Get<SimpleObject>();
                var obj2 = resolver.Get<SimpleObject>();

                Assert.Same(obj1, obj2);
            }
        }

        [Fact]
        public void ObjectInSingletonScopeAreDisposedWhenResolverDisposed()
        {
            var config = new ResolverConfig();
            config.Bind<DisposeTrackingObject>().ToSelf().InSingletonScope();

            DisposeTrackingObject obj;
            using (var resolver = config.ToResolver())
            {
                obj = resolver.Get<DisposeTrackingObject>();
            }

            Assert.True(obj.Disposed);
        }

        public sealed class DisposeTrackingObject : IDisposable
        {
            public bool Disposed { get; private set; }

            public void Dispose()
            {
                Disposed = true;
            }
        }

        public sealed class CustomeScopeObjectFactory : IObjectFactory
        {
            private static readonly ThreadLocal<Dictionary<IBinding, object>> Cache =
                new ThreadLocal<Dictionary<IBinding, object>>(() => new Dictionary<IBinding, object>());

            private readonly IBinding binding;

            private readonly IObjectFactory objectFactory;

            public CustomeScopeObjectFactory(IBinding binding, IObjectFactory objectFactory)
            {
                this.binding = binding;
                this.objectFactory = objectFactory;
            }

            public object Create()
            {
                if (Cache.Value.TryGetValue(binding, out var value))
                {
                    return value;
                }

                value = objectFactory.Create();
                Cache.Value[binding] = value;

                return value;
            }
        }

        public sealed class CustomScope : IScope
        {
            public IScope Copy(IComponentContainer components)
            {
                return this;
            }

            public IObjectFactory Create(IKernel kernel, IBinding binding, IObjectFactory factory)
            {
                return new CustomeScopeObjectFactory(binding, factory);
            }
        }
    }
}
