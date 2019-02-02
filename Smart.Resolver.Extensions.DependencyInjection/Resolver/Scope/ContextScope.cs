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
            throw new NotImplementedException();
        }

        public Func<object> Create(IKernel kernel, IBinding binding, Func<object> factory)
        {
            throw new NotImplementedException();
        }
    }
}
