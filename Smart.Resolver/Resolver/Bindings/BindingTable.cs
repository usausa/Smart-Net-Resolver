namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Smart.Collections.Concurrent;

    public class BindingTable : IBindingTable
    {
        private static readonly IBinding[] EmptyBindings = new IBinding[0];

        private readonly ThreadsafeTypeHashArrayMap<IBinding[]> table = new ThreadsafeTypeHashArrayMap<IBinding[]>();

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

        public IEnumerable<IBinding> EnumBindings()
        {
            return table.SelectMany(x => x.Value);
        }
    }
}
