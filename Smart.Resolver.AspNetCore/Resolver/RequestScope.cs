namespace Smart.Resolver
{
    using System;
    using System.Threading;

    using Microsoft.AspNetCore.Http;

    using Smart.ComponentModel;
    using Smart.Resolver.Bindings;
    using Smart.Resolver.Scopes;

    public class RequestScope : IScope
    {
        private readonly object sync = new object();

        private IHttpContextAccessor accessor;

        public IScope Copy(IComponentContainer components)
        {
            return this;
        }

        public object GetOrAdd(IKernel kernel, IBinding binding, Func<IBinding, object> factory)
        {
            if (accessor == null)
            {
                lock (sync)
                {
                    if (accessor == null)
                    {
                        var httpContextAccessor = kernel.Get<IHttpContextAccessor>();
                        Interlocked.MemoryBarrier();
                        accessor = httpContextAccessor;
                    }
                }
            }

            return HttpContextStorage.GetOrAdd(accessor.HttpContext, binding, factory);
        }
    }
}
