namespace Smart.Resolver
{
    using System.Collections.Generic;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    public interface IResolverConfig
    {
        IComponentContainer CreateComponentContainer();

        IEnumerable<IBinding> CreateBindings(IComponentContainer components);
    }
}
