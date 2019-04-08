namespace Smart.Resolver.Parameters
{
    using System;

    public sealed class CallbackParameter : IParameter
    {
        private readonly Func<IKernel, object> factory;

        public CallbackParameter(Func<IKernel, object> factory)
        {
            this.factory = factory;
        }

        public object Resolve(IKernel kernel)
        {
            return factory(kernel);
        }
    }
}
