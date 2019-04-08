namespace Smart.Resolver.Providers
{
    using System;

    using Smart.Resolver.Bindings;

    public sealed class CallbackProvider : IProvider
    {
        private readonly Func<IKernel, object> factory;

        public Type TargetType { get; }

        public CallbackProvider(Type type, Func<IKernel, object> factory)
        {
            TargetType = type;
            this.factory = factory;
        }

        public Func<object> CreateFactory(IKernel kernel, IBinding binding)
        {
            return () => factory(kernel);
        }
    }
}
