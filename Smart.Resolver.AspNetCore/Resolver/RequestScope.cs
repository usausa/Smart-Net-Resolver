namespace Smart.Resolver
{
    using Smart.Resolver.Scopes;

    public class RequestScope : IScope
    {
        public IScopeStorage GetStorage(IKernel kernel)
        {
            return kernel.Get<RequestScopeStorage>();
        }
    }
}
