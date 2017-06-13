namespace Smart.Resolver.Scopes
{
    using System;
    using System.Collections.Generic;

    using Smart.ComponentModel;

    using Smart.Resolver.Bindings;

    /// <summary>
    ///
    /// </summary>
    public class SingletonScopeStorage : DisposableObject, IScopeStorage
    {
        private readonly Dictionary<IBinding, object> cache = new Dictionary<IBinding, object>();

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
            lock (cache)
            {
                if (!cache.TryGetValue(binding, out object instance))
                {
                    instance = factory(binding);
                    cache[binding] = instance;
                }

                return instance;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void Clear()
        {
            lock (cache)
            {
                foreach (var instance in cache.Values)
                {
                    (instance as IDisposable)?.Dispose();
                }

                cache.Clear();
            }
        }
    }
}
