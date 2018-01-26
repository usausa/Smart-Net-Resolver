namespace Smart.Resolver.Providers
{
    using System;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public sealed class ConstantProvider : IProvider
    {
        private readonly object value;

        /// <summary>
        ///
        /// </summary>
        public Type TargetType => value.GetType();

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public ConstantProvider(object value)
        {
            this.value = value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public Func<object> CreateFactory(IKernel kernel, IBinding binding)
        {
            return () => value;
        }
    }
}
