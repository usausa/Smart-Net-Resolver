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

        public Func<IResolver, object> Create(IKernel kernel, IBinding binding, Func<IResolver, object> factory)
        {
            return resolver =>
            {
                var store = AsyncContext.Store;
                if (store is null)
                {
                    return factory(resolver);
                }

                if (!store.TryGetValue(binding, out var value))
                {
                    value = factory(resolver);
                    store[binding] = value;
                }

                return value;
            };
        }
    }
}
