namespace Smart.Resolver.Components;

using System;

internal sealed class ContainerSlot
{
    private readonly object sync = new();

    private object?[] entries = new object?[8];

    public object GetOrCreate(int index, Func<object> factory)
    {
        lock (sync)
        {
            if (index < entries.Length)
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
        Array.Copy(entries, 0, newEntries, 0, entries.Length);
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
