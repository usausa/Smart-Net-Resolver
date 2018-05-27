namespace Smart.Resolver.Injectors
{
    using System;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Metadatas;

    /// <summary>
    ///
    /// </summary>
    public interface IOldInjector
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <param name="metadata"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        bool IsTarget(IKernel kernel, IBinding binding, OldTypeMetadata metadata, Type type);

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <param name="metadata"></param>
        /// <param name="instance"></param>
        void Inject(IKernel kernel, IBinding binding, OldTypeMetadata metadata, object instance);
    }
}
