namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    public interface IScope
    {
        IScope Copy(IComponentContainer components);

        Func<IResolver, object> Create(IBinding binding, Func<object> factory);
    }
}
