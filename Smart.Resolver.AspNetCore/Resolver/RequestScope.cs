namespace Smart.Resolver
{
    using System.Threading;

    using Smart.Resolver.Scopes;

    public class RequestScope : IScope
    {
        private volatile RequestScopeStorage storage;

        public IScopeStorage GetStorage(IKernel kernel)
        {
#pragma warning disable 420
            if (storage == null)
            {
                Interlocked.CompareExchange(ref storage, kernel.Get<RequestScopeStorage>(), null);
            }
#pragma warning restore 420

            return storage;
        }
    }
}
