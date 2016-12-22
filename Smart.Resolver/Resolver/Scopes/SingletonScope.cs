namespace Smart.Resolver.Scopes
{
    using System.Threading;

    /// <summary>
    ///
    /// </summary>
    public class SingletonScope : IScope
    {
        private volatile SingletonScopeStorage storage;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Framework only")]
        public IScopeStorage GetStorage(IKernel kernel)
        {
#pragma warning disable 420
            if (storage == null)
            {
                Interlocked.CompareExchange(ref storage, kernel.Components.Get<SingletonScopeStorage>(), null);
            }
#pragma warning restore 420

            return storage;
        }
    }
}
