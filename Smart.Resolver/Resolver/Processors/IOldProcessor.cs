namespace Smart.Resolver.Processors
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public interface IOldProcessor
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsTarget(Type type);

        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        void Initialize(object instance);
    }
}
