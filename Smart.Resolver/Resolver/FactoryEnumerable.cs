namespace Smart.Resolver;

using System.Collections;
using System.Runtime.CompilerServices;

internal static class FactoryEnumerable
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IEnumerable<T> Create<T>(IResolver resolver, Func<IResolver, object>[] factories) =>
        factories.Length == 0 ? Array.Empty<T>() : new Enumerable<T>(resolver, factories);

    private sealed class Enumerable<T> : IEnumerable<T>, IEnumerator<T>
    {
        private readonly IResolver resolver;

        private readonly Func<IResolver, object>[] factories;

        private int index;

        private T current = default!;

        public Enumerable(IResolver resolver, Func<IResolver, object>[] factories)
        {
            this.resolver = resolver;
            this.factories = factories;
        }

#pragma warning disable IDE0032
        public T Current => current;
#pragma warning restore IDE0032

        object? IEnumerator.Current => current;

        public IEnumerator<T> GetEnumerator()
        {
            index = 1;
            return this;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public bool MoveNext()
        {
            var offset = index - 1;
            if ((uint)offset >= (uint)factories.Length)
            {
                index = -1;
                current = default!;
                return false;
            }

            current = UnsafeCast<T>(factories[offset](resolver));
            index++;
            return true;
        }

        public void Dispose()
        {
        }

        public void Reset() => throw new NotSupportedException();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static T UnsafeCast<T>(object obj)
    {
        if (typeof(T).IsValueType)
        {
            return (T)obj;
        }

        ref var r = ref Unsafe.As<object, T>(ref obj);
        return r;
    }
}
