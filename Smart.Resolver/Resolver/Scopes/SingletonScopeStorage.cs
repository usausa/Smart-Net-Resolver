namespace Smart.Resolver.Scopes
{
    using System;
    using System.Collections.Concurrent;

    using Smart.ComponentModel;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public class SingletonScopeStorage : DisposableObject, ISingletonScopeStorage
    {
        private readonly ConcurrentDictionary<IBinding, object> cache = new ConcurrentDictionary<IBinding, object>();

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
        /// <param name="instance"></param>
        public void Remember(IBinding binding, object instance)
        {
            cache[binding] = instance;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
        public object TryGet(IBinding binding)
        {
            object instance;
            return cache.TryGetValue(binding, out instance) ? instance : null;
        }

        /// <summary>
        ///
        /// </summary>
        public void Clear()
        {
            foreach (var instance in cache.Values)
            {
                (instance as IDisposable)?.Dispose();
            }

            cache.Clear();
        }
    }
}
