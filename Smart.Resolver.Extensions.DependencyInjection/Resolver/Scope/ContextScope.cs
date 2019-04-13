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

        public Func<IKernel, object> Create(IKernel kernel, IBinding binding, Func<IKernel, object> factory)
        {
            return k =>
            {
                var store = AsyncContext.Store;
                if (store is null)
                {
                    return factory(k);
                }

                if (!store.TryGetValue(binding, out var value))
                {
                    value = factory(k);
                    store[binding] = value;
                }

                return value;
            };
        }
    }
}
