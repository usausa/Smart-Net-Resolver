namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public interface IScopeStorage
    {
        object GetOrAdd(IBinding binding, Func<IBinding, object> factory);

        /// <summary>
        ///
        /// </summary>
        void Clear();
    }
}
