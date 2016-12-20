namespace Smart.Resolver.Parameters
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class CallbackParameter : IParameter
    {
        private readonly Func<IKernel, object> factory;

        /// <summary>
        ///
        /// </summary>
        /// <param name="factory"></param>
        public CallbackParameter(Func<IKernel, object> factory)
        {
            this.factory = factory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        public object Resolve(IKernel kernel)
        {
            return factory(kernel);
        }
    }
}
