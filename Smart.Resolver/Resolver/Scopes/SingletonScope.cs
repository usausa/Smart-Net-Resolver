namespace Smart.Resolver.Scopes
{
    using System;
    using System.Threading;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Disposables;

    /// <summary>
    ///
    /// </summary>
    public sealed class SingletonScope : IScope, IDisposable
    {
        private readonly object sync = new object();

        private object value;

        /// <summary>
        ///
        /// </summary>
        /// <param name="container"></param>
        public SingletonScope(IComponentContainer container)
        {
            container.Get<DisposableStorage>().Add(this);
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            if (value != null)
            {
                lock (sync)
                {
                    if (value != null)
                    {
                        (value as IDisposable)?.Dispose();
                        value = null;
                    }
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public object GetOrAdd(IKernel kernel, IBinding binding, Func<IBinding, object> factory)
        {
            if (value == null)
            {
                lock (sync)
                {
                    if (value == null)
                    {
                        var newObj = factory(binding);
                        Interlocked.MemoryBarrier();
                        value = newObj;
                    }
                }
            }

            return value;
        }
    }
}
