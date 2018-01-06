namespace Smart.Resolver
{
    using Microsoft.AspNetCore.Http;

    using Smart.Resolver.Bindings;
    using Smart.Resolver.Factories;

    public sealed class RequestScopeObjectFactory : IObjectFactory
    {
        private readonly IHttpContextAccessor accessor;

        private readonly IBinding binding;

        private readonly IObjectFactory objectFactory;

        public RequestScopeObjectFactory(IHttpContextAccessor accessor, IBinding binding, IObjectFactory objectFactory)
        {
            this.accessor = accessor;
            this.binding = binding;
            this.objectFactory = objectFactory;
        }

        public object Create()
        {
            return HttpContextStorage.GetOrAdd(accessor.HttpContext, binding, objectFactory);
        }
    }
}
