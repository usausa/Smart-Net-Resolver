namespace Smart.Resolver.Components
{
    using System;

    using System.Collections.Generic;

    public sealed class DisposableStorage : IDisposable
    {
        private readonly List<IDisposable> disposables = new List<IDisposable>();

        public void Add(IDisposable disposable)
        {
            disposables.Add(disposable);
        }

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
