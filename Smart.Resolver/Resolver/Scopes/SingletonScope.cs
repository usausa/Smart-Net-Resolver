namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Components;

    /// <summary>
    ///
    /// </summary>
    public sealed class SingletonScope : IScope, IDisposable
    {
        private object value;

        private Func<object> objectFactory;

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
            (value as IDisposable)?.Dispose();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="components"></param>
        /// <returns></returns>
        public IScope Copy(IComponentContainer components)
        {
            return new SingletonScope(components);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="kernel"></param>
        /// <param name="binding"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        public Func<object> Create(IKernel kernel, IBinding binding, Func<object> factory)
        {
            if (objectFactory == null)
            {
                value = factory();
                objectFactory = () => value;
            }

            return objectFactory;
        }
    }
}
