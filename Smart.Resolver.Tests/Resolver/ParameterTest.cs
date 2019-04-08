namespace Smart.Resolver
{
    using Smart.Resolver.Mocks;

    using Xunit;

    public class ParameterTest
    {
        [Fact]
        public void ObjectIsInjectedOnCreationWithConstantParameter()
        {
            var config = new ResolverConfig();
            var service = new Service();
            config.Bind<Controller>().ToSelf().WithConstructorArgument("service", service);

            using (var resolver = config.ToResolver())
            {
                var controller = resolver.Get<Controller>();

                Assert.Same(service, controller.Service);
            }
        }

        [Fact]
        public void ObjectIsInjectedOnCreationWithCallbackParameter()
        {
            var config = new ResolverConfig();
            config.Bind<IService>().To<Service>().InSingletonScope();
            config.Bind<Controller>().ToSelf().WithConstructorArgument("service", k => k.Get<IService>());

            using (var resolver = config.ToResolver())
            {
                var controller = resolver.Get<Controller>();
                var service = resolver.Get<IService>();

                Assert.Same(service, controller.Service);
            }
        }

        [Fact]
        public void ObjectIsInjectedWhenInjectWithConstantParameterOnCreation()
        {
            var config = new ResolverConfig();
            config.UsePropertyInjector();
            var injected = new SimpleObject();
            config.Bind<HasPropertyObject>().ToSelf().WithPropertyValue("Injected", injected);

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<HasPropertyObject>();

                Assert.Same(injected, obj.Injected);
            }
        }

        [Fact]
        public void ObjectIsInjectedWhenInjectWithCallbackParameterOnCreation()
        {
            var config = new ResolverConfig();
            config.UsePropertyInjector();
            config.Bind<SimpleObject>().ToSelf().InSingletonScope();
            config.Bind<HasPropertyObject>().ToSelf().WithPropertyValue("Injected", k => k.Get<SimpleObject>());

            using (var resolver = config.ToResolver())
            {
                var obj = resolver.Get<HasPropertyObject>();
                var injected = resolver.Get<SimpleObject>();

                Assert.Same(injected, obj.Injected);
            }
        }
    }
}
