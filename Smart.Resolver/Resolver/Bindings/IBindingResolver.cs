namespace Smart.Resolver.Bindings
{
    using System;
    using System.Collections.Generic;

    public interface IBindingResolver
    {
        IEnumerable<IBinding> Resolve(IResolverContext context, Type type);
    }
}
