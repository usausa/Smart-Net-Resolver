namespace Smart.Resolver.Providers
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CallbackProvider<T> : IProvider
    {
        private readonly Func<IResolver, T> factory;

        /// <summary>
        ///
        /// </summary>
        public Type TargetType => typeof(T);

        /// <summary>
        ///
        /// </summary>
        /// <param name="factory"></param>
        public CallbackProvider(Func<IResolver, T> factory)
        {
            this.factory = factory;
        }

        public object Create(IResolver resolver, IComponentContainer components, IBinding binding)
        {
            return factory(resolver);
        }
    }
}
