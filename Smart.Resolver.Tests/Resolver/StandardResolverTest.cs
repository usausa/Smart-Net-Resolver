namespace Smart.Resolver
{
    using Smart.Resolver.Mocks;

    using Xunit;

    public class StandardResolverTest
    {
        [Fact]
        public void SelfResolved()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<SmartResolver>();

                Assert.Same(resolver, obj);
            }
        }

        [Fact]
        public void SelfCanResolved()
        {
            var config = new ResolverConfig();

            using (var resolver = config.ToResolver())
            {
                var result = resolver.CanGet<SmartResolver>();

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
                Assert.NotNull(resolver.Get<SimpleObject>());
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
                Assert.Null(resolver.Get<IService>());
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
                var objects = resolver.GetAll<SimpleObject>();

                foreach (var obj in objects)
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
