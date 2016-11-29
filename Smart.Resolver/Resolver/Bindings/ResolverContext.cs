namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ResolverContext : IResolverContext
    {
        private readonly Dictionary<Type, IList<IBinding>> bindings;

        public ResolverContext(Dictionary<Type, IList<IBinding>> bindings)
        {
            this.bindings = bindings;
        }

        public IEnumerable<IBinding> FindBindings(Type type)
        {
            IList<IBinding> list;
            return bindings.TryGetValue(type, out list) ? list : Enumerable.Empty<IBinding>();
        }
    }
}
