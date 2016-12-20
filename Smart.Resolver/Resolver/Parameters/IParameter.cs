namespace Smart.Resolver.Parameters
{
    /// <summary>
    ///
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        object Resolve(IKernel kernel);
    }
}
