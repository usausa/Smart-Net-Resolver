namespace Smart.Resolver.Components;

internal static class ContainerIndexManager
{
#if NET9_0_OR_GREATER
    private static readonly Lock Sync = new();
#else
    private static readonly object Sync = new();
#endif

    private static int index;

    public static int Acquire()
    {
        lock (Sync)
        {
            return index++;
        }
    }
}
