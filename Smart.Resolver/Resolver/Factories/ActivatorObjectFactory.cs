namespace Smart.Resolver.Factories
{
    using System;

    using Smart.Reflection;

    public sealed class Activator0ObjectFactory : IObjectFactory
    {
        private readonly Action<object> processor;

        private readonly IActivator0 activator;

        public Activator0ObjectFactory(
            Action<object> processor,
            IActivator0 activator)
        {
            this.processor = processor;
            this.activator = activator;
        }

        public object Create()
        {
            var instance = activator.Create();
            processor(instance);
            return instance;
        }
    }

    public sealed class NoProcessActivator0ObjectFactory : IObjectFactory
    {
        private readonly IActivator0 activator;

        public NoProcessActivator0ObjectFactory(
            IActivator0 activator)
        {
            this.activator = activator;
        }

        public object Create()
        {
            return activator.Create();
        }
    }

    public sealed class Activator1ObjectFactory : IObjectFactory
    {
        private readonly Action<object> processor;

        private readonly IActivator1 activator;

        private readonly IObjectFactory argumentFactory1;

        public Activator1ObjectFactory(
            Action<object> processor,
            IActivator1 activator,
            IObjectFactory argumentFactory1)
        {
            this.processor = processor;
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
        }

        public object Create()
        {
            var instance = activator.Create(
                argumentFactory1.Create());
            processor(instance);
            return instance;
        }
    }

    public sealed class NoProcessActivator1ObjectFactory : IObjectFactory
    {
        private readonly IActivator1 activator;

        private readonly IObjectFactory argumentFactory1;

        public NoProcessActivator1ObjectFactory(
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

    public sealed class Activator2ObjectFactory : IObjectFactory
    {
        private readonly Action<object> processor;

        private readonly IActivator2 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        public Activator2ObjectFactory(
            Action<object> processor,
            IActivator2 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2)
        {
            this.processor = processor;
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
        }

        public object Create()
        {
            var instance = activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create());
            processor(instance);
            return instance;
        }
    }

    public sealed class NoProcessActivator2ObjectFactory : IObjectFactory
    {
        private readonly IActivator2 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        public NoProcessActivator2ObjectFactory(
            IActivator2 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2)
        {
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
        }

        public object Create()
        {
            return activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create());
        }
    }

    public sealed class Activator3ObjectFactory : IObjectFactory
    {
        private readonly Action<object> processor;

        private readonly IActivator3 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        public Activator3ObjectFactory(
            Action<object> processor,
            IActivator3 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3)
        {
            this.processor = processor;
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
        }

        public object Create()
        {
            var instance = activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create());
            processor(instance);
            return instance;
        }
    }

    public sealed class NoProcessActivator3ObjectFactory : IObjectFactory
    {
        private readonly IActivator3 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        public NoProcessActivator3ObjectFactory(
            IActivator3 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3)
        {
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
        }

        public object Create()
        {
            return activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create());
        }
    }

    public sealed class Activator4ObjectFactory : IObjectFactory
    {
        private readonly Action<object> processor;

        private readonly IActivator4 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        public Activator4ObjectFactory(
            Action<object> processor,
            IActivator4 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3,
            IObjectFactory argumentFactory4)
        {
            this.processor = processor;
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
            this.argumentFactory4 = argumentFactory4;
        }

        public object Create()
        {
            var instance = activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create(),
                argumentFactory4.Create());
            processor(instance);
            return instance;
        }
    }

    public sealed class NoProcessActivator4ObjectFactory : IObjectFactory
    {
        private readonly IActivator4 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        public NoProcessActivator4ObjectFactory(
            IActivator4 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3,
            IObjectFactory argumentFactory4)
        {
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
            this.argumentFactory4 = argumentFactory4;
        }

        public object Create()
        {
            return activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create(),
                argumentFactory4.Create());
        }
    }

    public sealed class Activator5ObjectFactory : IObjectFactory
    {
        private readonly Action<object> processor;

        private readonly IActivator5 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        private readonly IObjectFactory argumentFactory5;

        public Activator5ObjectFactory(
            Action<object> processor,
            IActivator5 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3,
            IObjectFactory argumentFactory4,
            IObjectFactory argumentFactory5)
        {
            this.processor = processor;
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
            this.argumentFactory4 = argumentFactory4;
            this.argumentFactory5 = argumentFactory5;
        }

        public object Create()
        {
            var instance = activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create(),
                argumentFactory4.Create(),
                argumentFactory5.Create());
            processor(instance);
            return instance;
        }
    }

    public sealed class NoProcessActivator5ObjectFactory : IObjectFactory
    {
        private readonly IActivator5 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        private readonly IObjectFactory argumentFactory5;

        public NoProcessActivator5ObjectFactory(
            IActivator5 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3,
            IObjectFactory argumentFactory4,
            IObjectFactory argumentFactory5)
        {
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
            this.argumentFactory4 = argumentFactory4;
            this.argumentFactory5 = argumentFactory5;
        }

        public object Create()
        {
            return activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create(),
                argumentFactory4.Create(),
                argumentFactory5.Create());
        }
    }

    public sealed class Activator6ObjectFactory : IObjectFactory
    {
        private readonly Action<object> processor;

        private readonly IActivator6 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        private readonly IObjectFactory argumentFactory5;

        private readonly IObjectFactory argumentFactory6;

        public Activator6ObjectFactory(
            Action<object> processor,
            IActivator6 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3,
            IObjectFactory argumentFactory4,
            IObjectFactory argumentFactory5,
            IObjectFactory argumentFactory6)
        {
            this.processor = processor;
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
            var instance = activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create(),
                argumentFactory4.Create(),
                argumentFactory5.Create(),
                argumentFactory6.Create());
            processor(instance);
            return instance;
        }
    }

    public sealed class NoProcessActivator6ObjectFactory : IObjectFactory
    {
        private readonly IActivator6 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        private readonly IObjectFactory argumentFactory5;

        private readonly IObjectFactory argumentFactory6;

        public NoProcessActivator6ObjectFactory(
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

    public sealed class Activator7ObjectFactory : IObjectFactory
    {
        private readonly Action<object> processor;

        private readonly IActivator7 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        private readonly IObjectFactory argumentFactory5;

        private readonly IObjectFactory argumentFactory6;

        private readonly IObjectFactory argumentFactory7;

        public Activator7ObjectFactory(
            Action<object> processor,
            IActivator7 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3,
            IObjectFactory argumentFactory4,
            IObjectFactory argumentFactory5,
            IObjectFactory argumentFactory6,
            IObjectFactory argumentFactory7)
        {
            this.processor = processor;
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
            this.argumentFactory4 = argumentFactory4;
            this.argumentFactory5 = argumentFactory5;
            this.argumentFactory6 = argumentFactory6;
            this.argumentFactory7 = argumentFactory7;
        }

        public object Create()
        {
            var instance = activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create(),
                argumentFactory4.Create(),
                argumentFactory5.Create(),
                argumentFactory6.Create(),
                argumentFactory7.Create());
            processor(instance);
            return instance;
        }
    }

    public sealed class NoProcessActivator7ObjectFactory : IObjectFactory
    {
        private readonly IActivator7 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        private readonly IObjectFactory argumentFactory5;

        private readonly IObjectFactory argumentFactory6;

        private readonly IObjectFactory argumentFactory7;

        public NoProcessActivator7ObjectFactory(
            IActivator7 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3,
            IObjectFactory argumentFactory4,
            IObjectFactory argumentFactory5,
            IObjectFactory argumentFactory6,
            IObjectFactory argumentFactory7)
        {
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
            this.argumentFactory4 = argumentFactory4;
            this.argumentFactory5 = argumentFactory5;
            this.argumentFactory6 = argumentFactory6;
            this.argumentFactory7 = argumentFactory7;
        }

        public object Create()
        {
            return activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create(),
                argumentFactory4.Create(),
                argumentFactory5.Create(),
                argumentFactory6.Create(),
                argumentFactory7.Create());
        }
    }

    public sealed class Activator8ObjectFactory : IObjectFactory
    {
        private readonly Action<object> processor;

        private readonly IActivator8 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        private readonly IObjectFactory argumentFactory5;

        private readonly IObjectFactory argumentFactory6;

        private readonly IObjectFactory argumentFactory7;

        private readonly IObjectFactory argumentFactory8;

        public Activator8ObjectFactory(
            Action<object> processor,
            IActivator8 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3,
            IObjectFactory argumentFactory4,
            IObjectFactory argumentFactory5,
            IObjectFactory argumentFactory6,
            IObjectFactory argumentFactory7,
            IObjectFactory argumentFactory8)
        {
            this.processor = processor;
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
            this.argumentFactory4 = argumentFactory4;
            this.argumentFactory5 = argumentFactory5;
            this.argumentFactory6 = argumentFactory6;
            this.argumentFactory7 = argumentFactory7;
            this.argumentFactory8 = argumentFactory8;
        }

        public object Create()
        {
            var instance = activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create(),
                argumentFactory4.Create(),
                argumentFactory5.Create(),
                argumentFactory6.Create(),
                argumentFactory7.Create(),
                argumentFactory8.Create());
            processor(instance);
            return instance;
        }
    }

    public sealed class NoProcessActivator8ObjectFactory : IObjectFactory
    {
        private readonly IActivator8 activator;

        private readonly IObjectFactory argumentFactory1;

        private readonly IObjectFactory argumentFactory2;

        private readonly IObjectFactory argumentFactory3;

        private readonly IObjectFactory argumentFactory4;

        private readonly IObjectFactory argumentFactory5;

        private readonly IObjectFactory argumentFactory6;

        private readonly IObjectFactory argumentFactory7;

        private readonly IObjectFactory argumentFactory8;

        public NoProcessActivator8ObjectFactory(
            IActivator8 activator,
            IObjectFactory argumentFactory1,
            IObjectFactory argumentFactory2,
            IObjectFactory argumentFactory3,
            IObjectFactory argumentFactory4,
            IObjectFactory argumentFactory5,
            IObjectFactory argumentFactory6,
            IObjectFactory argumentFactory7,
            IObjectFactory argumentFactory8)
        {
            this.activator = activator;
            this.argumentFactory1 = argumentFactory1;
            this.argumentFactory2 = argumentFactory2;
            this.argumentFactory3 = argumentFactory3;
            this.argumentFactory4 = argumentFactory4;
            this.argumentFactory5 = argumentFactory5;
            this.argumentFactory6 = argumentFactory6;
            this.argumentFactory7 = argumentFactory7;
            this.argumentFactory8 = argumentFactory8;
        }

        public object Create()
        {
            return activator.Create(
                argumentFactory1.Create(),
                argumentFactory2.Create(),
                argumentFactory3.Create(),
                argumentFactory4.Create(),
                argumentFactory5.Create(),
                argumentFactory6.Create(),
                argumentFactory7.Create(),
                argumentFactory8.Create());
        }
    }

    public sealed class ActivatorObjectFactory : IObjectFactory
    {
        private readonly Action<object> processor;

        private readonly IActivator activator;

        private readonly IObjectFactory[] argumentFactories;

        public ActivatorObjectFactory(
            Action<object> processor,
            IActivator activator,
            params IObjectFactory[] argumentFactories)
        {
            this.processor = processor;
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

            var instance = activator.Create(arguments);
            processor(instance);
            return instance;
        }
    }

    public sealed class NoProcessActivatorObjectFactory : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly IObjectFactory[] argumentFactories;

        public NoProcessActivatorObjectFactory(
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
