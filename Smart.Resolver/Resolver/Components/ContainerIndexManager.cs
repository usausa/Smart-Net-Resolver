namespace Smart.Resolver.Components;

internal static class ContainerIndexManager
{
    private static readonly Lock Sync = new();

    private static int index;

    public static int Acquire()
    {
        lock (Sync)
        {
            return index++;
        }
    }
}
