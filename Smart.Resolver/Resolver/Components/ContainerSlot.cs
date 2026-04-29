namespace Smart.Resolver.Components;

using System.Runtime.CompilerServices;

internal sealed class ContainerSlot
{
#if NET9_0_OR_GREATER
    private readonly Lock sync = new();
#else
    private readonly object sync = new();
#endif

    private object?[] entries = new object?[8];

    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public object GetOrCreate(int index, Func<object> factory)
    {
        lock (sync)
        {
            if ((uint)index < (uint)entries.Length)
            {
                var obj = entries[index];
                if (obj is null)
                {
                    obj = factory();
                    entries[index] = obj;
                }

                return obj;
            }
            else
            {
                Grow(index);

                var obj = factory();
                entries[index] = obj;

                return obj;
            }
        }
    }

    private void Grow(int index)
    {
        var newEntries = new object[((index >> 3) << 3) + 8];
        entries.CopyTo(newEntries, 0);
        entries = newEntries;
    }

    public void Clear()
    {
        lock (sync)
        {
            foreach (var entry in entries)
            {
                (entry as IDisposable)?.Dispose();
            }

            Array.Clear(entries, 0, entries.Length);
        }
    }
}
