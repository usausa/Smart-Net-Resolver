using System;

namespace Smart.Resolver
{
    using System.Collections.Generic;
    using System.Threading;

    using Smart.Resolver.Bindings;

    internal static class AsyncContext
    {
        private static readonly AsyncLocal<Dictionary<IBinding, object>> AsyncStore = new AsyncLocal<Dictionary<IBinding, object>>();

        public static Dictionary<IBinding, object> Store => AsyncStore.Value;

        public static void Initializable()
        {
            AsyncStore.Value = new Dictionary<IBinding, object>();
        }

        public static void Clear()
        {
            var store = AsyncStore.Value;

            foreach (var instance in store.Values)
            {
                (instance as IDisposable)?.Dispose();
            }

            store.Clear();
        }
    }
}
