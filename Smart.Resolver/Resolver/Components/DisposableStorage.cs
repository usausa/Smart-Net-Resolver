namespace Smart.Resolver.Components;

public sealed class DisposableStorage : IDisposable
{
    private readonly List<IDisposable> disposables = [];

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
