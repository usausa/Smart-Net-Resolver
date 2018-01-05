namespace Smart.Resolver
{
    using System;
    using Smart.Resolver.Mocks;

    using Xunit;

    /// <summary>
    ///
    /// </summary>
    public class StandardResolverTest
    {
        [Fact]
        public void SelfResolved()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<IResolver>();

                Assert.Same(resolver, obj);
            }
        }

        [Fact]
        public void SelfTryResolved()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.TryGet<IResolver>(out var result);

                Assert.True(result);
                Assert.Same(resolver, obj);
            }
        }

        [Fact]
        public void SelfCanResolved()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var result = resolver.CanGet<IResolver>();

                Assert.True(result);
            }
        }

        [Fact]
        public void ObjectIsResolvedWhenBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<SimpleObject>();

                Assert.NotNull(obj);
            }
        }

        [Fact]
        public void ObjectIsResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig().UseAutoBinding();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<SimpleObject>();

                Assert.NotNull(obj);
            }
        }

        [Fact]
        public void ObjectIsTryResolvedWhenBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.TryGet<SimpleObject>(out var result);

                Assert.True(result);
                Assert.NotNull(obj);
            }
        }

        [Fact]
        public void ObjectIsTryResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig().UseAutoBinding();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.TryGet<SimpleObject>(out var result);

                Assert.True(result);
                Assert.NotNull(obj);
            }
        }

        [Fact]
        public void ObjectCanResolvedWhenBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                var result = resolver.CanGet<SimpleObject>();

                Assert.True(result);
            }
        }

        [Fact]
        public void ObjectCanResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig().UseAutoBinding();

            using (var resolver = config.ToResolver())
            {
                var result = resolver.CanGet<SimpleObject>();

                Assert.True(result);
            }
        }

        [Fact]
        public void ObjectIsResolvedWhenMultiBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                resolver.Get<SimpleObject>();
            }
        }

        [Fact]
        public void ObjectIsTryResolvedWhenMultiBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                resolver.TryGet<SimpleObject>(out var result);

                Assert.True(result);
            }
        }

        [Fact]
        public void ObjectCanResolvedWhenMultiBindIsExist()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();
            config.Bind<SimpleObject>().ToSelf().InTransientScope();

            using (var resolver = config.ToResolver())
            {
                var result = resolver.CanGet<SimpleObject>();

                Assert.True(result);
            }
        }

        [Fact]
        public void InterfaceIsNotResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                Assert.Throws<InvalidOperationException>(() => resolver.Get<IService>());
            }
        }

        [Fact]
        public void InterfaceIsNotTryResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                resolver.TryGet<IService>(out var result);

                Assert.False(result);
            }
        }

        [Fact]
        public void InterfaceIsNotCanResolvedWhenBindIsNotExist()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var result = resolver.CanGet<IService>();

                Assert.False(result);
            }
        }

        [Fact]
        public void ObjectIsAllResolvedWhenMultiBinding()
        {
            var config = new ResolverConfig();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("foo");
            config.Bind<SimpleObject>().ToSelf().InSingletonScope().Named("bar");

            using (var resolver = config.ToResolver())
            {
                var objs = resolver.GetAll<SimpleObject>();

                foreach (var obj in objs)
                {
                    Assert.Equal(typeof(SimpleObject), obj.GetType());
                }
            }
        }

        [Fact]
        public void UseCaseForWebControllerAndService()
        {
            var config = new ResolverConfig();
            config.Bind<IService>().To<Service>().InSingletonScope();
            config.Bind<Controller>().ToSelf();

            using (var resolver = config.ToResolver())
            {
                var controller1 = (Controller)resolver.Get(typeof(Controller));
                var controller2 = (Controller)resolver.Get(typeof(Controller));

                Assert.NotSame(controller1, controller2);
                Assert.Same(controller1.Service, controller2.Service);
            }
        }
    }
}
