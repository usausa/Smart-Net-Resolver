namespace Smart.Resolver.Builder
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public interface IObjectFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <returns></returns>
        object Create(IKernel kernel);
    }
}
