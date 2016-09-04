namespace Smart.Resolver.Providers
{
    using System;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CallbackProvider<T> : IProvider
    {
        private readonly Func<IKernel, T> factory;

        /// <summary>
        ///
        /// </summary>
        /// <param name="factory"></param>
        public CallbackProvider(Func<IKernel, T> factory)
        {
            this.factory = factory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public object Create(IKernel kernel, IBinding binding)
        {
            return factory(kernel);
        }
    }
}
