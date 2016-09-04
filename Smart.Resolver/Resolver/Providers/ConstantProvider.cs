namespace Smart.Resolver.Providers
{
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
        /// <param name="value"></param>
        public ConstantProvider(T value)
        {
            this.value = value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public object Create(IKernel kernel, IBinding binding)
        {
            return value;
        }
    }
}
