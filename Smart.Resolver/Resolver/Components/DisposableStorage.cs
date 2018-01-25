namespace Smart.Resolver.Components
{
    using System;

    using System.Collections.Generic;

    /// <summary>
    ///
    /// </summary>
    public sealed class DisposableStorage : IDisposable
    {
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="disposable"></param>
        public void Add(IDisposable disposable)
        {
            disposables.Add(disposable);
        }

        /// <summary>
        ///
        /// </summary>
        public void Dispose()
        {
            foreach (var disposable in disposables)
            {
                disposable.Dispose();
            }

            disposables.Clear();
        }
    }
}
