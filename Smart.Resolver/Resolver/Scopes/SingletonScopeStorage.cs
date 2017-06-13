namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.Collections.Generic.Concurrent;
    using Smart.ComponentModel;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public class SingletonScopeStorage : DisposableObject, IScopeStorage
    {
        private readonly ConcurrentHashArrayMap<IBinding, object> cache = new ConcurrentHashArrayMap<IBinding, object>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposing"></param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Clear();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="binding"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public object GetOrAdd(IBinding binding, Func<IBinding, object> factory)
        {
            if (cache.TryGetValue(binding, out object instance))
            {
                return instance;
            }

            return cache.AddIfNotExist(binding, factory);
        }

        /// <summary>
        ///
        /// </summary>
        public void Clear()
        {
            foreach (var pair in cache)
            {
                (pair.Value as IDisposable)?.Dispose();
            }

            cache.Clear();
        }
    }
}
