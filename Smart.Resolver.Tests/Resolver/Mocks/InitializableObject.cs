namespace Smart.Resolver.Mocks;

public sealed class InitializableObject : IInitializable
{
    public int InitializedCount { get; private set; }

    public void Initialize()
    {
        InitializedCount++;
    }
}
