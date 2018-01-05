namespace Smart.Resolver.Providers
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Factories;

    /// <summary>
    ///
    /// </summary>
    public sealed class CallbackProvider : IProvider
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
        /// <param name="components"></param>
        /// <returns></returns>
        public IProvider Copy(IComponentContainer components)
        {
            return this;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
        public IObjectFactory CreateFactory(IKernel kernel, IBinding binding)
        {
            return new CallbackObjectFactory(kernel, factory);
        }
    }
}
