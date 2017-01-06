namespace Smart.Resolver.Providers
{
    using System;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public class CallbackProvider : IProvider
    {
        private readonly Func<IKernel, object> factory;

        /// <summary>
        ///
        /// </summary>
        public Type TargetType { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <param name="factory"></param>
        public CallbackProvider(Type type, Func<IKernel, object> factory)
        {
            TargetType = type;
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
