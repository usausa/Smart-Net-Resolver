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

    private List<IDisposable>? disposables;

    [SkipLocalsInit]
    [MethodImpl(MethodImplOptions.AggressiveInlining | MethodImplOptions.AggressiveOptimization)]
    public object GetOrCreate(int index, IResolver resolver, Func<IResolver, object> factory)
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
                    obj = factory(resolver);
                    slot = obj;
                }

                return obj;
            }
            else
            {
                Grow(index);

                var obj = factory(resolver);
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

    public void AddDisposable(IDisposable disposable)
    {
        lock (sync)
        {
            disposables ??= [];
            disposables.Add(disposable);
        }
    }

    public void Clear()
    {
        lock (sync)
        {
            var list = disposables;
            if (list is not null)
            {
                // Reverse creation order, before container-scoped instances
                for (var i = list.Count - 1; i >= 0; i--)
                {
                    list[i].Dispose();
                }

                list.Clear();
            }

            foreach (var entry in entries.AsSpan())
            {
                (entry as IDisposable)?.Dispose();
            }

            entries.AsSpan().Clear();
        }
    }
}
