namespace Smart.Resolver.Providers
{
    using System;

    using Smart.Resolver.Bindings;

    public sealed class CallbackProvider<T> : IProvider
    {
        private readonly Func<IResolver, T> factory;

        public Type TargetType { get; }

        public CallbackProvider(Func<IResolver, T> factory)
        {
            this.factory = factory;
            TargetType = typeof(T);
        }

        public Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding)
        {
            return r => factory(r)!;
        }
    }
}
