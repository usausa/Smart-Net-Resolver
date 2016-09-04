namespace Smart.Resolver.Scopes
{
    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public interface IScopeStorage
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="instance"></param>
        void Remember(IBinding binding, object instance);

        /// <summary>
        ///
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        object TryGet(IBinding binding);

        /// <summary>
        ///
        /// </summary>
        void Clear();
    }
}
