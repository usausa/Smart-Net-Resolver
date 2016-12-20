namespace Smart.Resolver
{
    using System.Threading;

    using Smart.ComponentModel;
    using Smart.Resolver.Scopes;

    public class RequestScope : IScope
    {
        private volatile RequestScopeStorage storage;

        public IScopeStorage GetStorage(IKernel kernel)
        {
            if (storage == null)
            {
#pragma warning disable 420
                Interlocked.CompareExchange(ref storage, kernel.Get<RequestScopeStorage>(), null);
#pragma warning restore 420
            }

            return storage;
        }
    }
}
