﻿namespace Smart.Resolver.Factories
{
    using Smart.Reflection;

    public sealed class ActivatorObjectFactory0 : IObjectFactory
    {
        private readonly IActivator activator;

        public ActivatorObjectFactory0(
            IActivator activator)
        {
            this.activator = activator;
        }

        public object Create(IKernel kernel)
        {
            return activator.Create(kernel);
        }
    }

    public sealed class ActivatorObjectFactory1 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly IObjectFactory argument1Factory;

        public ActivatorObjectFactory1(
            IActivator activator,
            IObjectFactory argument1Factory)
        {
            this.activator = activator;
            this.argument1Factory = argument1Factory;
        }

        public object Create(IKernel kernel)
        {
            return activator.Create(
                argument1Factory.Create(kernel));
        }
    }

    public sealed class ActivatorObjectFactory2 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        public ActivatorObjectFactory2(
            IActivator activator,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory)
        {
            this.activator = activator;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
        }

        public object Create(IKernel kernel)
        {
            return activator.Create(
                argument1Factory.Create(kernel),
                argument2Factory.Create(kernel));
        }
    }

    public sealed class ActivatorObjectFactory3 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        private readonly IObjectFactory argument3Factory;

        public ActivatorObjectFactory3(
            IActivator activator,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory,
            IObjectFactory argument3Factory)
        {
            this.activator = activator;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
        }

        public object Create(IKernel kernel)
        {
            return activator.Create(
                argument1Factory.Create(kernel),
                argument2Factory.Create(kernel),
                argument3Factory.Create(kernel));
        }
    }

    public sealed class ActivatorObjectFactory4 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        private readonly IObjectFactory argument3Factory;

        private readonly IObjectFactory argument4Factory;

        public ActivatorObjectFactory4(
            IActivator activator,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory,
            IObjectFactory argument3Factory,
            IObjectFactory argument4Factory)
        {
            this.activator = activator;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
            this.argument4Factory = argument4Factory;
        }

        public object Create(IKernel kernel)
        {
            return activator.Create(
                argument1Factory.Create(kernel),
                argument2Factory.Create(kernel),
                argument3Factory.Create(kernel),
                argument4Factory.Create(kernel));
        }
    }

    public sealed class ActivatorObjectFactory5 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        private readonly IObjectFactory argument3Factory;

        private readonly IObjectFactory argument4Factory;

        private readonly IObjectFactory argument5Factory;

        public ActivatorObjectFactory5(
            IActivator activator,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory,
            IObjectFactory argument3Factory,
            IObjectFactory argument4Factory,
            IObjectFactory argument5Factory)
        {
            this.activator = activator;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
            this.argument4Factory = argument4Factory;
            this.argument5Factory = argument5Factory;
        }

        public object Create(IKernel kernel)
        {
            return activator.Create(
                argument1Factory.Create(kernel),
                argument2Factory.Create(kernel),
                argument3Factory.Create(kernel),
                argument4Factory.Create(kernel),
                argument5Factory.Create(kernel));
        }
    }

    public sealed class ActivatorObjectFactory6 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        private readonly IObjectFactory argument3Factory;

        private readonly IObjectFactory argument4Factory;

        private readonly IObjectFactory argument5Factory;

        private readonly IObjectFactory argument6Factory;

        public ActivatorObjectFactory6(
            IActivator activator,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory,
            IObjectFactory argument3Factory,
            IObjectFactory argument4Factory,
            IObjectFactory argument5Factory,
            IObjectFactory argument6Factory)
        {
            this.activator = activator;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
            this.argument4Factory = argument4Factory;
            this.argument5Factory = argument5Factory;
            this.argument6Factory = argument6Factory;
        }

        public object Create(IKernel kernel)
        {
            return activator.Create(
                argument1Factory.Create(kernel),
                argument2Factory.Create(kernel),
                argument3Factory.Create(kernel),
                argument4Factory.Create(kernel),
                argument5Factory.Create(kernel),
                argument6Factory.Create(kernel));
        }
    }

    public sealed class ActivatorObjectFactory7 : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly IObjectFactory argument1Factory;

        private readonly IObjectFactory argument2Factory;

        private readonly IObjectFactory argument3Factory;

        private readonly IObjectFactory argument4Factory;

        private readonly IObjectFactory argument5Factory;

        private readonly IObjectFactory argument6Factory;

        private readonly IObjectFactory argument7Factory;

        public ActivatorObjectFactory7(
            IActivator activator,
            IObjectFactory argument1Factory,
            IObjectFactory argument2Factory,
            IObjectFactory argument3Factory,
            IObjectFactory argument4Factory,
            IObjectFactory argument5Factory,
            IObjectFactory argument6Factory,
            IObjectFactory argument7Factory)
        {
            this.activator = activator;
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
            this.argument4Factory = argument4Factory;
            this.argument5Factory = argument5Factory;
            this.argument6Factory = argument6Factory;
            this.argument7Factory = argument7Factory;
        }

        public object Create(IKernel kernel)
        {
            return activator.Create(
                argument1Factory.Create(kernel),
                argument2Factory.Create(kernel),
                argument3Factory.Create(kernel),
                argument4Factory.Create(kernel),
                argument5Factory.Create(kernel),
                argument6Factory.Create(kernel),
                argument7Factory.Create(kernel));
        }
    }

    public sealed class ActivatorObjectFactory8 : IObjectFactory
    {
        private readonly IActivator activator;

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
            this.argument1Factory = argument1Factory;
            this.argument2Factory = argument2Factory;
            this.argument3Factory = argument3Factory;
            this.argument4Factory = argument4Factory;
            this.argument5Factory = argument5Factory;
            this.argument6Factory = argument6Factory;
            this.argument7Factory = argument7Factory;
            this.argument8Factory = argument8Factory;
        }

        public object Create(IKernel kernel)
        {
            return activator.Create(
                argument1Factory.Create(kernel),
                argument2Factory.Create(kernel),
                argument3Factory.Create(kernel),
                argument4Factory.Create(kernel),
                argument5Factory.Create(kernel),
                argument6Factory.Create(kernel),
                argument7Factory.Create(kernel),
                argument8Factory.Create(kernel));
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

        public object Create(IKernel kernel)
        {
            var arguments = new object[argumentFactories.Length];
            for (var i = 0; i < argumentFactories.Length; i++)
            {
                arguments[i] = argumentFactories[i].Create(kernel);
            }

            return activator.Create(arguments);
        }
    }
}
