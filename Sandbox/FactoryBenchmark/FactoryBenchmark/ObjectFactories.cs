namespace FactoryBenchmark
{
    public interface IObjectFactory
    {
        object Create();
    }

    public sealed class ConstantObjectFactory : IObjectFactory
    {
        private readonly object value;

        public ConstantObjectFactory(object value)
        {
            this.value = value;
        }

        public object Create()
        {
            return value;
        }
    }

    public sealed class ActivatorObjectFactory0 : IObjectFactory
    {
        private readonly IActivator0 activator;

        public ActivatorObjectFactory0(
            IActivator0 activator)
        {
            this.activator = activator;
        }

        public object Create()
        {
            return activator.Create();
        }
    }

    public sealed class ActivatorObjectFactory1 : IObjectFactory
    {
        private readonly IActivator1 activator;

        private readonly IObjectFactory argumentFactory1;

        public ActivatorObjectFactory1(
            IActivator1 activator,
            IObjectFactory argumentFactory1)
        {
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
        }

        public object Create()
        {
            return activator.Create(
                argumentFactory1.Create());
        }
    }

    public sealed class ActivatorObjectFactory6 : IObjectFactory
    {
        private readonly IActivator6 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        private readonly IObjectFactory argumentFactory5;

        private readonly IObjectFactory argumentFactory6;

        public ActivatorObjectFactory6(
            IActivator6 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3,
            IObjectFactory argumentFactory4,
            IObjectFactory argumentFactory5,
            IObjectFactory argumentFactory6)
        {
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
            this.argumentFactory4 = argumentFactory4;
            this.argumentFactory5 = argumentFactory5;
            this.argumentFactory6 = argumentFactory6;
        }

        public object Create()
        {
            return activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create(),
                argumentFactory4.Create(),
                argumentFactory5.Create(),
                argumentFactory6.Create());
        }
    }

    public sealed class ActivatorObjectFactory : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly IObjectFactory[] argumentFactories;

        public ActivatorObjectFactory(
            IActivator activator,
            params IObjectFactory[] argumentFactories)
        {
            this.activator = activator;
            this.argumentFactories = argumentFactories;
        }

        public object Create()
        {
            var arguments = new object[argumentFactories.Length];
            for (var i = 0; i < argumentFactories.Length; i++)
            {
                arguments[i] = argumentFactories[i].Create();
            }

            return activator.Create(arguments);
        }
    }
}
