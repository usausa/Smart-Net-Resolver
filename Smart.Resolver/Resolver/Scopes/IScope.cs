namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.ComponentModel;

    public interface IScope
    {
        IScope Copy(IComponentContainer components);

        Func<IResolver, object> Create(Func<object> factory);
    }
}
