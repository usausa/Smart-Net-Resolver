namespace Smart.Resolver.Metadatas
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public interface IMetadataFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        TypeMetadata GetMetadata(Type type);
    }
}
