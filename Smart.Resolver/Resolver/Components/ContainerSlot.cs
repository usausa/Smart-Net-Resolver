namespace Smart.Resolver.Components;

using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

internal sealed class ContainerSlot
{
#if NET9_0_OR_GREATER
    private readonly Lock sync = new();
#else
    private readonly object sync = new();
#endif

    private object?[] entries = new object?[8];

    [SkipLocalsInit]
    [MethodImpl(MethodImplOptions.AggressiveOptimization)]
    public object GetOrCreate(int index, Func<object> factory)
    {
        lock (sync)
        {
            var entriesLocal = entries;
            if ((uint)index < (uint)entriesLocal.Length)
            {
                ref var slot = ref Unsafe.Add(ref MemoryMarshal.GetArrayDataReference(entriesLocal), index);
                var obj = slot;
                if (obj is null)
                {
                    obj = factory();
                    slot = obj;
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
        var newEntries = new object?[((index >> 3) << 3) + 8];
        entries.AsSpan().CopyTo(newEntries);
        entries = newEntries;
    }

    public void Clear()
    {
        lock (sync)
        {
            foreach (var entry in entries.AsSpan())
            {
                (entry as IDisposable)?.Dispose();
            }

            entries.AsSpan().Clear();
        }
    }
}
