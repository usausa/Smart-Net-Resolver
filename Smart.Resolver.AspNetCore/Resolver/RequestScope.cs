namespace Smart.Resolver
{
    using System.Threading;

    using Smart.ComponentModel;
    using Smart.Resolver.Scopes;

    public class RequestScope : IScope
    {
        private volatile RequestScopeStorage storage;

        public IScopeStorage GetStorage(IResolver resolver)
        {
            return resolver.Get<RequestScopeStorage>();
        }

        public IScopeStorage GetStorage(IResolver resolver, IComponentContainer components)
        {
            if (storage == null)
            {
#pragma warning disable 420
                Interlocked.CompareExchange(ref storage, resolver.Get<RequestScopeStorage>(), null);
#pragma warning restore 420
            }

            return storage;
        }
    }
}
