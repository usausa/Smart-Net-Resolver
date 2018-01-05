namespace Smart.Resolver.Factories
{
    using System;

    public sealed class CallbackObjectFactory : IObjectFactory
    {
        private readonly IKernel kernel;

        private readonly Func<IKernel, object> factory;

        public CallbackObjectFactory(IKernel kernel, Func<IKernel, object> factory)
        {
            this.kernel = kernel;
            this.factory = factory;
        }

        public object Create()
        {
            return factory(kernel);
        }
    }
}
