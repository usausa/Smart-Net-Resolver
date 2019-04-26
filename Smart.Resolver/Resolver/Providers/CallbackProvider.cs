namespace Smart.Resolver.Providers
{
    using System;

    using Smart.Resolver.Bindings;

    public sealed class CallbackProvider : IProvider
    {
        private readonly Func<IResolver, object> factory;

        public Type TargetType { get; }

        public CallbackProvider(Type type, Func<IResolver, object> factory)
        {
            TargetType = type;
            this.factory = factory;
        }

        public Func<IResolver, object> CreateFactory(IKernel kernel, IBinding binding)
        {
            return r => factory(r);
        }
    }
}
