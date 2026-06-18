namespace Smart.Resolver.Components;

using System.Runtime.InteropServices;

public sealed class DisposableStorage : IDisposable
{
    private readonly List<IDisposable> disposables = [];

    public void Add(IDisposable disposable)
    {
        disposables.Add(disposable);
    }

    public void Dispose()
    {
        foreach (var disposable in CollectionsMarshal.AsSpan(disposables))
        {
            disposable.Dispose();
        }

        disposables.Clear();
    }
}
