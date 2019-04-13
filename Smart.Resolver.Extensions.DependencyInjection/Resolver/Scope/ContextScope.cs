namespace Smart.Resolver.Scope
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Scopes;

    public sealed class ContextScope : IScope
    {
        public IScope Copy(IComponentContainer components)
        {
            return this;
        }

        public Func<IResolver, object> Create(IBinding binding, Func<object> factory)
        {
            return resolver =>
            {
                var store = AsyncContext.Store;
                if (store is null)
                {
                    return factory();
                }

                if (!store.TryGetValue(binding, out var value))
                {
                    value = factory();
                    store[binding] = value;
                }

                return value;
            };
        }
    }
}
