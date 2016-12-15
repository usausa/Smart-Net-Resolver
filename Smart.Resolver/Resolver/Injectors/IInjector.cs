namespace Smart.Resolver.Injectors
{
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Metadatas;

    /// <summary>
    ///
    /// </summary>
    public interface IInjector
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="resolver"></param>
        /// <param name="binding"></param>
        /// <param name="metadata"></param>
        /// <param name="instance"></param>
        void Inject(IResolver resolver, IBinding binding, TypeMetadata metadata, object instance);
    }
}
