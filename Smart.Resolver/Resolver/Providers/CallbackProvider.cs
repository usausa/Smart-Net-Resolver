namespace Smart.Resolver.Providers
{
    using System;

    using Smart.Resolver.Bindings;

    public sealed class CallbackProvider<T> : IProvider
    {
        private readonly Func<IResolver, T> factory;

        public Type TargetType => typeof(T);

        public CallbackProvider(Func<IResolver, T> factory)
        {
            this.factory = factory;
        }

        public Func<IResolver, object> CreateFactory(IKernel kernel, IBinding binding)
        {
            return (Func<IResolver, object>)(object)factory;
        }
    }
}
