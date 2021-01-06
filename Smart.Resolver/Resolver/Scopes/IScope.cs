namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.ComponentModel;

    public interface IScope
    {
        IScope Copy(ComponentContainer components);

        Func<IResolver, object> Create(Func<object> factory);
    }
}
