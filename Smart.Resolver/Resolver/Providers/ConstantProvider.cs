namespace Smart.Resolver.Providers
{
    using System;

    using Smart.Resolver.Bindings;

    public sealed class ConstantProvider<T> : IProvider
    {
        private readonly Func<IResolver, T> factory;

        public Type TargetType => typeof(T);

        public ConstantProvider(T value)
        {
            factory = r => value;
        }

        public Func<IResolver, object> CreateFactory(IKernel kernel, IBinding binding)
        {
            return (Func<IResolver, object>)(object)factory;
        }
    }
}
