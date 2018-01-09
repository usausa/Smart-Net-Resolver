namespace Smart.Resolver.Factories
{
    using System;

    using Smart.Reflection;

    public sealed class NoArtumentsActivatorObjectFactory : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly Action<object> processor;

        public NoArtumentsActivatorObjectFactory(
            IActivator activator,
            Action<object> processor)
        {
            this.activator = activator;
            this.processor = processor;
        }

        public object Create()
        {
            var instance = activator.Create();
            processor(instance);
            return instance;
        }
    }

    public sealed class NoArtumentsWithoutProcessActivatorObjectFactory : IObjectFactory
    {
        private readonly IActivator activator;

        public NoArtumentsWithoutProcessActivatorObjectFactory(
            IActivator activator)
        {
            this.activator = activator;
        }

        public object Create()
        {
            return activator.Create();
        }
    }

    public sealed class ActivatorObjectFactory : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly Action<object> processor;

        private readonly IObjectFactory[] argumentFactories;

        public ActivatorObjectFactory(
            IActivator activator,
            Action<object> processor,
            params IObjectFactory[] argumentFactories)
        {
            this.activator = activator;
            this.processor = processor;
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

    public sealed class WithoutProcessActivatorObjectFactory : IObjectFactory
    {
        private readonly IActivator activator;

        private readonly IObjectFactory[] argumentFactories;

        public WithoutProcessActivatorObjectFactory(
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
