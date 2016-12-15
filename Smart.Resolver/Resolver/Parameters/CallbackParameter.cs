namespace Smart.Resolver.Parameters
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public class CallbackParameter : IParameter
    {
        private readonly Func<IResolver, object> factory;

        /// <summary>
        ///
        /// </summary>
        /// <param name="factory"></param>
        public CallbackParameter(Func<IResolver, object> factory)
        {
            this.factory = factory;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="resolver"></param>
        /// <returns></returns>
        public object Resolve(IResolver resolver)
        {
            return factory(resolver);
        }
    }
}
