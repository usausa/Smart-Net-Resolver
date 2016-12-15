namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Concurrent;

    public class BindingTable : IBindingTable
    {
        private static readonly IBinding[] EmptyBindings = new IBinding[0];

        private readonly ConcurrentDictionary<Type, IBinding[]> table = new ConcurrentDictionary<Type, IBinding[]>();

        public void Add(Type type, IBinding[] bindings)
        {
            table[type] = bindings;
        }

        public IBinding[] GetOrAdd(Type type, Func<Type, IBinding[]> factory)
        {
            return table.GetOrAdd(type, factory);
        }

        public IBinding[] FindBindings(Type type)
        {
            IBinding[] bindings;
            return table.TryGetValue(type, out bindings) ? bindings : EmptyBindings;
        }
    }
}
