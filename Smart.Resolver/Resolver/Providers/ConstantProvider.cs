namespace Smart.Resolver.Providers
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ConstantProvider<T> : IProvider
    {
        private readonly T value;

        /// <summary>
        ///
        /// </summary>
        public Type TargetType => value.GetType();

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public ConstantProvider(T value)
        {
            this.value = value;
        }

        public object Create(IResolver resolver, IComponentContainer components, IBinding binding)
        {
            return value;
        }
    }
}
