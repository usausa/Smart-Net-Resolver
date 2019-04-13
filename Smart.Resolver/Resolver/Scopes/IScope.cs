namespace Smart.Resolver.Scopes
{
    using System;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;

    public interface IScope
    {
        IScope Copy(IComponentContainer components);

        Func<IKernel, object> Create(IKernel kernel, IBinding binding, Func<IKernel, object> factory);
    }
}
