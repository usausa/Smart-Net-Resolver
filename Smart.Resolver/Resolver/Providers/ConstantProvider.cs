namespace Smart.Resolver.Providers
{
    using System;

    using Smart.Resolver.Bindings;

    public sealed class ConstantProvider : IProvider
    {
        private readonly object value;

        public Type TargetType => value.GetType();

        public ConstantProvider(object value)
        {
            this.value = value;
        }

        public Func<IResolver, object> CreateFactory(IKernel kernel, Binding binding)
        {
            return r => value;
        }
    }
}
