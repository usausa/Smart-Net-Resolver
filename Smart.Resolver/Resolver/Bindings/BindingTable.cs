namespace Smart.Resolver.Bindings
{
    using System;

    using Smart.Collections.Generic.Concurrent;

    public class BindingTable : IBindingTable
    {
        private static readonly IBinding[] EmptyBindings = new IBinding[0];

        private readonly ConcurrentHashArrayMap<Type, IBinding[]> table = new ConcurrentHashArrayMap<Type, IBinding[]>();

        public void Add(Type type, IBinding[] bindings)
        {
            table.AddIfNotExist(type, bindings);
        }

        public IBinding[] GetOrAdd(Type type, Func<Type, IBinding[]> factory)
        {
            if (table.TryGetValue(type, out IBinding[] bindings))
            {
                return bindings;
            }

            return table.AddIfNotExist(type, factory);
        }

        public IBinding[] FindBindings(Type type)
        {
            return table.TryGetValue(type, out IBinding[] bindings) ? bindings : EmptyBindings;
        }
    }
}
