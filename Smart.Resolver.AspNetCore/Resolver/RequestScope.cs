namespace Smart.Resolver
{
    using Microsoft.AspNetCore.Http;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Factories;
    using Smart.Resolver.Scopes;

    public sealed class RequestScope : IScope
    {
        public IScope Copy(IComponentContainer components)
        {
            return this;
        }

        public IObjectFactory Convert(IKernel kernel, IBinding binding, IObjectFactory factory)
        {
            return new RequestScopeObjectFactory(kernel.Get<IHttpContextAccessor>(), binding, factory);
        }
    }
}
