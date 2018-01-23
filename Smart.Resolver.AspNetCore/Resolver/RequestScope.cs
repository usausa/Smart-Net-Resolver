namespace Smart.Resolver
{
    using System;

    using Microsoft.AspNetCore.Http;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Scopes;

    public sealed class RequestScope : IScope
    {
        public IScope Copy(IComponentContainer components)
        {
            return this;
        }

        public Func<object> Create(IKernel kernel, IBinding binding, Func<object> factory)
        {
            return Create(kernel.Get<IHttpContextAccessor>(), binding, factory);
        }

        private static Func<object> Create(IHttpContextAccessor accessor, IBinding binding, Func<object> factory)
        {
            return () => HttpContextStorage.GetOrAdd(accessor.HttpContext, binding, factory);
        }
    }
}
