namespace Smart.Resolver.Factories
{
    using Smart.Reflection;

    public sealed class ActivatorObjectFactory0 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly ActivateHelper helper;

        public ActivatorObjectFactory0(
            IActivator activator,
            ActivateHelper helper)
        {
            this.activator = activator;
            this.helper = helper;
        }

        public object Create()
        {
            var instance = activator.Create();
            helper?.Process(instance);
            return instance;
        }
    }

    public sealed class ActivatorObjectFactory1 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly ActivateHelper helper;

        private readonly IObjectFactory argument1Factory;

        public ActivatorObjectFactory1(
            IActivator activator,
            ActivateHelper helper,
            IObjectFactory argument1Factory)
        {
            this.activator = activator;
            this.helper = helper;
            this.argument1Factory = argument1Factory;
        }

        public object Create()
        {
            var instance = activator.Create(
                argument1Factory.Create());
            helper?.Process(instance);
            return instance;
        }
    }

    public sealed class ActivatorObjectFactory2 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly ActivateHelper helper;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        public ActivatorObjectFactory2(
            IActivator activator,
            ActivateHelper helper,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory)
        {
            this.activator = activator;
            this.helper = helper;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
        }

        public object Create()
        {
            var instance = activator.Create(
                argument1Factory.Create(),
                argument2Factory.Create());
            helper?.Process(instance);
            return instance;
        }
    }

    public sealed class ActivatorObjectFactory3 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly ActivateHelper helper;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        private readonly IObjectFactory argument3Factory;

        public ActivatorObjectFactory3(
            IActivator activator,
            ActivateHelper helper,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory,
            IObjectFactory argument3Factory)
        {
            this.activator = activator;
            this.helper = helper;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
        }

        public object Create()
        {
            var instance = activator.Create(
                argument1Factory.Create(),
                argument2Factory.Create(),
                argument3Factory.Create());
            helper?.Process(instance);
            return instance;
        }
    }

    public sealed class ActivatorObjectFactory4 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly ActivateHelper helper;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        private readonly IObjectFactory argument3Factory;

        private readonly IObjectFactory argument4Factory;

        public ActivatorObjectFactory4(
            IActivator activator,
            ActivateHelper helper,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory,
            IObjectFactory argument3Factory,
            IObjectFactory argument4Factory)
        {
            this.activator = activator;
            this.helper = helper;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
            this.argument4Factory = argument4Factory;
        }

        public object Create()
        {
            var instance = activator.Create(
                argument1Factory.Create(),
                argument2Factory.Create(),
                argument3Factory.Create(),
                argument4Factory.Create());
            helper?.Process(instance);
            return instance;
        }
    }

    public sealed class ActivatorObjectFactory5 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly ActivateHelper helper;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        private readonly IObjectFactory argument3Factory;

        private readonly IObjectFactory argument4Factory;

        private readonly IObjectFactory argument5Factory;

        public ActivatorObjectFactory5(
            IActivator activator,
            ActivateHelper helper,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory,
            IObjectFactory argument3Factory,
            IObjectFactory argument4Factory,
            IObjectFactory argument5Factory)
        {
            this.activator = activator;
            this.helper = helper;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
            this.argument4Factory = argument4Factory;
            this.argument5Factory = argument5Factory;
        }

        public object Create()
        {
            var instance = activator.Create(
                argument1Factory.Create(),
                argument2Factory.Create(),
                argument3Factory.Create(),
                argument4Factory.Create(),
                argument5Factory.Create());
            helper?.Process(instance);
            return instance;
        }
    }

    public sealed class ActivatorObjectFactory6 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly ActivateHelper helper;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        private readonly IObjectFactory argument3Factory;

        private readonly IObjectFactory argument4Factory;

        private readonly IObjectFactory argument5Factory;

        private readonly IObjectFactory argument6Factory;

        public ActivatorObjectFactory6(
            IActivator activator,
            ActivateHelper helper,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory,
            IObjectFactory argument3Factory,
            IObjectFactory argument4Factory,
            IObjectFactory argument5Factory,
            IObjectFactory argument6Factory)
        {
            this.activator = activator;
            this.helper = helper;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
            this.argument4Factory = argument4Factory;
            this.argument5Factory = argument5Factory;
            this.argument6Factory = argument6Factory;
        }

        public object Create()
        {
            var instance = activator.Create(
                argument1Factory.Create(),
                argument2Factory.Create(),
                argument3Factory.Create(),
                argument4Factory.Create(),
                argument5Factory.Create(),
                argument6Factory.Create());
            helper?.Process(instance);
            return instance;
        }
    }

    public sealed class ActivatorObjectFactory7 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly ActivateHelper helper;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        private readonly IObjectFactory argument3Factory;

        private readonly IObjectFactory argument4Factory;

        private readonly IObjectFactory argument5Factory;

        private readonly IObjectFactory argument6Factory;

        private readonly IObjectFactory argument7Factory;

        public ActivatorObjectFactory7(
            IActivator activator,
            ActivateHelper helper,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory,
            IObjectFactory argument3Factory,
            IObjectFactory argument4Factory,
            IObjectFactory argument5Factory,
            IObjectFactory argument6Factory,
            IObjectFactory argument7Factory)
        {
            this.activator = activator;
            this.helper = helper;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
            this.argument4Factory = argument4Factory;
            this.argument5Factory = argument5Factory;
            this.argument6Factory = argument6Factory;
            this.argument7Factory = argument7Factory;
        }

        public object Create()
        {
            var instance = activator.Create(
                argument1Factory.Create(),
                argument2Factory.Create(),
                argument3Factory.Create(),
                argument4Factory.Create(),
                argument5Factory.Create(),
                argument6Factory.Create(),
                argument7Factory.Create());
            helper?.Process(instance);
            return instance;
        }
    }

    public sealed class ActivatorObjectFactory8 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly ActivateHelper helper;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        private readonly IObjectFactory argument3Factory;

        private readonly IObjectFactory argument4Factory;

        private readonly IObjectFactory argument5Factory;

        private readonly IObjectFactory argument6Factory;

        private readonly IObjectFactory argument7Factory;

        private readonly IObjectFactory argument8Factory;

        public ActivatorObjectFactory8(
            IActivator activator,
            ActivateHelper helper,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory,
            IObjectFactory argument3Factory,
            IObjectFactory argument4Factory,
            IObjectFactory argument5Factory,
            IObjectFactory argument6Factory,
            IObjectFactory argument7Factory,
            IObjectFactory argument8Factory)
        {
            this.activator = activator;
            this.helper = helper;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
            this.argument4Factory = argument4Factory;
            this.argument5Factory = argument5Factory;
            this.argument6Factory = argument6Factory;
            this.argument7Factory = argument7Factory;
            this.argument8Factory = argument8Factory;
        }

        public object Create()
        {
            var instance = activator.Create(
                argument1Factory.Create(),
                argument2Factory.Create(),
                argument3Factory.Create(),
                argument4Factory.Create(),
                argument5Factory.Create(),
                argument6Factory.Create(),
                argument7Factory.Create(),
                argument8Factory.Create());
            helper?.Process(instance);
            return instance;
        }
    }

    public sealed class ActivatorObjectFactory : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly ActivateHelper helper;

        private readonly IObjectFactory[] argumentFactories;

        public ActivatorObjectFactory(
            IActivator activator,
            ActivateHelper helper,
            params IObjectFactory[] argumentFactories)
        {
            this.activator = activator;
            this.helper = helper;
            this.argumentFactories = argumentFactories;
        }

        public object Create()
        {
            var arguments = new object[argumentFactories.Length];
            for (var i = 0; i < argumentFactories.Length; i++)
            {
                arguments[i] = argumentFactories[i].Create();
            }

            var instance = activator.Create(arguments);
            helper?.Process(instance);
            return instance;
        }
    }
}
