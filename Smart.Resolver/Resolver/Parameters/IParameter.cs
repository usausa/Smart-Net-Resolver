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
        /// <param name="resolver"></param>
        /// <returns></returns>
        object Resolve(IResolver resolver);
    }
}
