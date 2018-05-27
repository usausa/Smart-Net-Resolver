namespace Smart.Resolver.Metadatas
{
    using System;

    /// <summary>
    ///
    /// </summary>
    public interface IOldMetadataFactory
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        OldTypeMetadata GetMetadata(Type type);
    }
}
