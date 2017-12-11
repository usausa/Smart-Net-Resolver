namespace Smart.Resolver.Parameters
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ConstantParameter : IParameter
    {
        private readonly object value;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public ConstantParameter(object value)
        {
            this.value = value;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        public object Resolve(IKernel kernel)
        {
            return value;
        }
    }
}
